var http = require('http'),
    querystring = require('querystring'),
    util = require('util'),
    xml2js = require(__dirname + '/xml2js');

var host = 'soiduplaan.tallinn.ee';

module.exports = function(action, params, callback) {
  if(!action) throw new Error('Action missing!');
  params = params || {};
  params.a = 'p.' + action;
  params.t = 'xml';
  params.l = 'ee';
  
  var query = '/?' + querystring.stringify(params);
  util.log('Fetching XML from http://' + host + query);
  var req = http.request({ host: host, port: 80, path: query, method: 'GET' }, function(res) {
    if(res.statusCode != 200) throw res.statusCode;
    var buffer = '';
    res.on('data', function(chunk) {
      buffer += chunk;
    });
    
    res.on('end', function() {
      var parser = new xml2js.Parser();
      parser.on('end', function(result) {
        callback && callback(result, buffer.length);
      });
      parser.parseString(buffer);
    });
  });
  req.setHeader('User-Agent', 'Mozilla/5.0');
  req.end();
};