var download = require(__dirname + '/downloader'),
    querystring = require('querystring'),
    fs = require('fs'),
    util = require('util');

/**
 * Create new scraper
 * Set params
 * @constructor
 */
var Scraper = function(settings) {
  this.log = settings.log || true;
  this.rate = settings.rate || 5;
  this.cacheDir = __dirname + '/../cache';
  this.outputDir = __dirname + '/../public';
  this.xmlCounter = 0;
  this.jsonCounter = 0;
  this.queueList = [];
  this.runningJobs = 0;
  
  try {
    fs.readdirSync(this.cacheDir);
  } catch(e) {
    fs.mkdirSync(this.cacheDir, 0777);
  }
  
  try {
    fs.readdirSync(this.outputDir);
  } catch(e) {
    fs.mkdirSync(this.outputDir, 0777);
  }
};

/**
 * Output structure
 */
Scraper.prototype.out = {
  routes: [],
  stops: []
};

/**
 * Predefined constants
 */
Scraper.prototype.dict = {
  //Entry level 0
  transport_id: [
    'bus',
    'trolleybus',
    'tram',
    'suburban_bus',
    'commercial_bus', 
    'train'
  ],
  
  levels: [
    'routes',
    'directions',
    'stops',
    'schedule',
    'timing'
  ],
  
  dayMap: {
    '1': 0,
    '2': 1,
    '4': 2,
    '8': 3,
    '16': 4,
    '32': 5,
    '64': 6
  }
};

/**
 * Rate limiting structure
 * Has the same signature as fetch
 * Limited by rate, sequential
 */
Scraper.prototype.queue = function(action, params, callback) {
  var that = this;
  if(action && callback) {
    this.queueList.push({ action: action, params: params, callback: callback });
  }

  if(this.queueList.length > 0 && this.runningJobs < this.rate) {
    var job = this.queueList.shift();
    this.runningJobs++;
    this.fetch(job.action, job.params, function(data) {
      that.runningJobs--;
      job.callback && job.callback(data);
      if(that.queueList.length > 0) {
        that.queue();
      }
    });
  }
};

/**
 * Reads JSON from soiduplaan.tallinn.ee
 * With 1-day disk caching
 * Counts total size difference between XML and JSON
 * @param {String} p.action param
 * @param {Object} soiduplaan additional params
 * @param {Function} completion callback
 */
Scraper.prototype.fetch = function(action, params, callback) {
  console.time('Fetch');
  var that = this;
  var d = new Date();
  var dateString = (d.getDate()-1) + '-' + (d.getMonth()+1) + '-' + d.getFullYear();
  var paramsString = querystring.stringify(params);
  var path = this.cacheDir + '/' + dateString + '-action=' + action 
      + '&' + paramsString;
  
  fs.stat(path + '.json', function(err, stats) {
    if(stats) {
      util.log('Fetching from disk: action=' + action + '&' + querystring.stringify(params));
      //Read from cache
      fs.readFile(path + '.json', function (err, data) {
        if (err) throw err;
        var dataObj = JSON.parse(data);
        console.timeEnd('Fetch');
        callback && callback(dataObj);
      });
      
    } else {
      util.log('Fetching from web: action=' + action + '&' + querystring.stringify(params));
      download(action, params, function(data, xmlLength) {
        delete data.transport_styles;
        var serializedJSON = JSON.stringify(data);
        
        //Count XML size
        that.xmlCounter += xmlLength;
        
        //Count JSON size
        that.jsonCounter += serializedJSON.length;
        
        //Save human readable and regular JSON to disk for caching
        fs.writeFileSync(path + '.json', serializedJSON);
        fs.writeFileSync(path + '-human.json', util.inspect(data, false, null));
        
        console.timeEnd('Fetch');
        callback && callback(data);
      });
      
    }
  });
};

/**
 * Prints counters
 */
Scraper.prototype.printStat = function() {
  //Statistics
  console.log('JSON Size: ' + this.jsonCounter + 'bytes');
  console.log('XML Size: ' + this.xmlCounter + ' bytes');
};


/**
 * Cyclic sync struct
 * @param {Number} Cycle length
 * @param {Function} Function with code, gets an cb for calling next iteration
 * @param {Function} callback for ending the callback
 */
Scraper.prototype.cycle = function(cLength, codeFunc, doneCb) {

  if(!cLength || cLength == 1) {
    codeFunc(function() {
      doneCb();
    }, 0);
    return;
  }
  
  var i = 0;
  var cb = function() {
    if(i != cLength-1) {
      i++;
      codeFunc(cb, i);
    } else {
      doneCb();
    }
  }
   
  codeFunc(cb, i);
};
/**
 * Dumps object as JSON to public
 * @param {String} output key
 */
Scraper.prototype.writeObj = function(key) {
  var data = JSON.stringify(this.out[key] || {});
  fs.writeFile(this.outputDir + '/' + key + '.json', data);
};

/************************
 * Parser logic methods *
 ************************/

Scraper.prototype.parseRoute = function(obj, cb) {
  var dayMap = this.dict.dayMap;
  for(var i = 0; i < obj.length; i++) {
  
    var route = {
      number: obj[i].number,
      title: obj[i].direction,
      lowfloor: obj[i].lowfloor,
      vehicle: obj[i].vehicle
    };
    var schedules = [];
    var days = obj[i].days.day;
    
    for(var u = 0; u < days.length; u++) {
      if(days[u].schedule_id) {
        schedules.push({
          day: dayMap[days[u].day_num],
          id: days[u].schedule_id,
          directionid: days[u].direction_id,
          validfrom: days[u].valid
        });
      }
    }
    route['schedules'] = schedules;
    this.out.routes.push(route);
    
    if(i == obj.length-1) {
      cb && cb();
    }
  }
};

Scraper.prototype.parseDirections = function(obj, schedule_id, chunkId, cb) {
  var that = this;
  var schedule = {
    schedule_id: schedule_id,
    directions: []
  };
  
  that.cycle(obj.length, function(next, i) {
    if(!obj.length) {
      var tmpObj = obj;
      obj = [ tmpObj ];
    }

    var direction = {
      id: obj[i]['@'].direction_id,
      name: obj[i].name,
      stops : []
    }
    
    var dirStops = obj[i].stops.stop;
    that.cycle(dirStops.length, function(next2, u) {
      if(!dirStops.length) {
        var tmpObj = dirStops;
        dirStops = [ tmpObj ];
      }
      var stopSched = {
        id: dirStops[u]['@'].stop_id
      };
      that.queue('schedule', {
        'schedule_id': schedule_id,
        'direction_id': obj[i]['@'].direction_id,
        'stop_id': dirStops[u]['@'].stop_id
      }, function(data) {
        var days = data.days.day;
        var parsedDays = [];
        for(var p = 0; p < days.length; p++) {
          var parseDay = {
            v: days[p]['@'].day,
            h: []
          }
          for(var p1 = 0; p1 < days[p].hour.length; p1++) {
            var currentHour = {
              v: days[p].hour[p1]['@'].hr,
              m: []
            }
            for(var p2 = 0; p2 < days[p].hour[p1].minutes.length; p2++) {
              var currentMinute = days[p].hour[p1].minutes[p2];
              currentHour.m.push({
                v: currentMinute['#'],
                id: currentMinute['@'].id,
                lf: currentMinute['@'].lowfloor
              });
            }
            parseDay.h.push(currentHour);
          }

          parsedDays.push(parseDay);
        }       
        
        stopSched['days'] = parsedDays;
        direction.stops.push(stopSched);
        next2();
      });
    }, function() {
      schedule.directions.push(direction);
      next();
    });    
  }, function() {
    that.out['schedule-' + chunkId].push(schedule);
    cb();
  });
};


/**
 * Parses stops from cache
 * @param {Function} callback with object
 */
Scraper.prototype.getCSVStops = function(callback) {
  download('cache', 'stops.txt', function(data) {
    var stops = [];
    var lines = data.split('\n');
    for(var i = 0; i < lines.length; i++) {
      if(lines[i] != '') {
        var stop = lines[i].split(';');
        stops.push({
          stop_id: stop[0],
          title: stop[1],
          latitude: stop[2],
          longitude: stop[3]
        });
      }
    }
    callback && callback(stops);
  });
};

/**
 * Parses stops from cache
 * @param {Function} callback with object
 */
Scraper.prototype.getCSVRoutes = function(callback) {
  download('cache', 'routes.txt', function(data) {
    var routes = [];
    var lines = data.split('\n');
    for(var i = 0; i < lines.length; i++) {
      var route = lines[i].split(';');
      var routestops = [];
      for(var u = 6; u < route.length; u++) {
        routestops.push(route[u]);
      }
      
      routes.push({
        city: route[0],
        vehicle: route[1],
        provider: route[2],
        number: route[3],
        direction_id: route[4],
        title: route[5],
        stops: routestops
      });
    }
    callback && callback(routes);
  });
};




/**
 * Init method
 */
Scraper.prototype.start = function() {
  var that = this;
  var transports = this.dict.transport_id;
  var out = this.out; 

  //Gather misc data under generic.json
  this.queue('routes', {
    'transport_id': 'bus'
  }, function(data) {
    out.generic = {};
    var types = data.types.type;
    out.generic.types = [];
    for(var i = 0; i < types.length; i++) {
      out.generic.types.push({
        type: types[i]['#'],
        routes: types[i]['@'].routes,
        city: types[i]['@'].city
      });
    }
    
    that.writeObj('generic');
  });
  
  //Step 1 - Fetch all routes
  this.cycle(transports.length, function(cb, i) {
    that.queue('routes', {
      'transport_id': transports[i]
    }, function(data) {
      that.parseRoute(data.routes.route, function() {
        cb();
      });
    });
  }, function() {
    routesDone();
  });

  //Step 2 - Fetch all stops
  var routesDone = function() {
    that.writeObj('routes');
    that.getCSVStops(function(data) {
      var currentStop = { stops: [] };
      for(var i = 0; i < data.length; i++) {
        //Next stopgroup
        if(currentStop.title && data[i].title != '') {
          that.out.stops.push(currentStop);
          currentStop = { stops: [] };
        }
        
        //Set the title
        if(data[i].title != '') {
          currentStop.title = data[i].title;
        }
        
        delete data[i].title;
        
        currentStop.stops.push({
          id: data[i].stop_id,
          lat: data[i].latitude,
          lon: data[i].longitude
        });
        
      }
      that.writeObj('stops');
      stopsDone();
    });
  };
  
  //Step 3 - Fetch all directions
  var stopsDone = function() {
    var chunkId = 0;
    that.out['schedule-' + chunkId] = [];

    that.cycle(that.out.routes.length, function(cb, i) {
      if(i % 20 == 0 && i != 0;) {
        that.writeObj('schedule-' + chunkId);
        delete that.out['schedule-' + chunkId];
        chunkId++;
        that.out['schedule-' + chunkId] = [];
      }
      var schedules = that.out.routes[i].schedules;
      that.cycle(schedules.length, function(cb2, u) {
        that.queue('directions', {
        'schedule_id': schedules[u].id
        }, function(data) {
          if(data.directions) {
          that.parseDirections(data.directions.direction, schedules[u].id, chunkId, function() {
            cb();
          });
          } else {
            cb();
          }
        });
        
      }, function() {
      
      });
    }, function() {
      //POST PROCESS - load all chunks
      
      //that.writeObj('schedule');
    });
  };
};



//Expose as module
module.exports = Scraper;