const http = require('node:http');
const fs = require('node:fs/promises');
const path = require('node:path');

const files = new Set(['index.html', 'list.js', 'edit.html', 'edit.js', 'edit.css', 'styles.css']);
const types = {'.html': 'text/html; charset=utf-8', '.js': 'text/javascript; charset=utf-8', '.css': 'text/css; charset=utf-8'};

http.createServer(async (request, response) => {
  const name = new URL(request.url, 'http://localhost:3000').pathname.slice(1) || 'index.html';
  if (!files.has(name)) {
    response.writeHead(404);
    response.end('Not found');
    return;
  }
  try {
    const body = await fs.readFile(path.join(__dirname, name));
    response.writeHead(200, {'Content-Type': types[path.extname(name)]});
    response.end(body);
  } catch {
    response.writeHead(500);
    response.end('Server error');
  }
}).listen(3000, 'localhost', () => console.log('Практична 26–27: http://localhost:3000'));
