// server.js
// This is the main file for the server
const express = require('express');
const app = express();
const http = require('http');
const path = require('path');
const port = 3001;// Use the whole root as static files to be able to serve the html file and

const sql = require('mssql');

const cors = require('cors');


const cartasWebPath = path.join('C:', 'VS', 'PROJECTS', 'RETRY', 'END'); 
// the build 
app.use(express.static(path.join(cartasWebPath, '/')));
// Send html on '/'path
app.get('/', (req, res) => {res.sendFile(path.join(cartasWebPath, + '/index.html'));})

// SQL Server configuration
/*var config = {
    "user": "sa", // Database username
    "password": "12345", // Database password
    "server": "BRUNOLAP", // Server IP address
    "database": "KDD", // Database name
    "options": {
        "encrypt": false // Disable encryption
    }
}*/

app.use(cors());

app.use(express.json());


// Create the server and listen on port
http.createServer(app).listen(port, () => {console.log(`Server running on localhost:${port}`);});
