const express = require('express');
const path = require('path');
const compression = require('compression');
const fs = require('fs');
const app = express();
const port = 3000;

// Enable GZIP compression for all requests
app.use(compression());

// Middleware to serve pre-compressed .gz files if they exist
app.get('*.js', (req, res, next) => {
  const gzPath = path.join(__dirname, 'public', req.url + '.gz');
  if (fs.existsSync(gzPath)) {
    req.url = req.url + '.gz'; // change the request to point to the .gz file
    res.set('Content-Encoding', 'gzip');
  }
  next();
});

// Serve static files from the "public" directory
app.use(express.static(path.join(__dirname, 'public')));

// Default route
app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, 'public', 'index.html'));
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});
