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
Scraper.prototype.out = {};

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
  ]
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
  var dateString = d.getDate() + '-' + (d.getMonth()+1) + '-' + d.getFullYear();
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
        fs.writeFile(path + '.json', serializedJSON);
        fs.writeFile(path + '-human.json', util.inspect(data, false, null));
        
        console.timeEnd('Fetch');
        callback && callback(data);
      });
      
    }
  });
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

Scraper.prototype.parseRoute = function(obj) {
  this.out.routes = [];
  for(var i = 0; i < obj.length; i++) {
    this.out.routes[i] = obj[i];
    /*this.queue('schedule', {
      'schedule_id': obj[i].schedule_id
    }, function(data) {
    });*/ 
  }
  console.log(this.jsonCounter);
  console.log(this.xmlCounter);

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
    out.generic.types = {};
    for(var i = 0; i < types.length; i++) {
      out.generic.types[types[i]['#']] = {
        routes: types[i]['@'].routes,
        city: types[i]['@'].city
      }
    }
    
    that.writeObj('generic');
  });
  
  //Step 1 - Fetch all transports
  for(var i = 0; i < transports.length; i++) {
    this.queue('routes', {
      'transport_id': transports[i]
    }, function(data) {
      that.parseRoute(data.routes.route);
    }); 
  }
  

};



//Expose as module
module.exports = Scraper;