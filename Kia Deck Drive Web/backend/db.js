// db.js
const sql = require('mssql');

// Configure your database connection
const config = {
    user: 'sa', // your database username
    password: '12345', // your database password
    server: 'BRUNOLAP', // e.g. 'localhost' or an IP address
    database: 'KDD', // your database name
    options: {
        encrypt: true, // Use this if you're on Azure
        trustServerCertificate: true, // Use this for local dev / self-signed certs
    }
};

// Function to connect to the database
const connectToDatabase = async () => {
    try {
        await sql.connect(config);
        console.log('Connected to the database!');
    } catch (err) {
        console.error('Database connection failed:', err);
    }
};

module.exports = { sql, connectToDatabase };
