const express = require('express');
const cors = require('cors');
const { sql, connectToDatabase } = require('./db');
const dotenv = require('dotenv');

dotenv.config();

const app = express();
const PORT = process.env.PORT || 5000;

// Middleware
app.use(cors());
app.use(express.json()); // For parsing application/json
app.use(express.urlencoded({ extended: true })); // For parsing application/x-www-form-urlencoded

// Connect to the database
connectToDatabase();


// Definir ruta para obtener el conteo de usuarios
// Ruta para obtener la cantidad de usuarios
// Endpoint para obtener el número de usuarios
app.get('/api/usuarios/count', async (req, res) => {
    try {
      const result = await new sql.Request().query('SELECT COUNT(*) AS count FROM Usuarios');
      res.json(result.recordset[0]); // Enviar la respuesta con el conteo
    } catch (error) {
      console.error('Error al ejecutar la consulta:', error);
      res.status(500).send('Error en el servidor');
    }
  });

//Ruta para obtener la cantidad de usarios conectados
app.get('/api/usuarios/conectados', async (req, res) => {
    try {
      const result = await new sql.Request().query("SELECT COUNT(*) AS count FROM Usuarios WHERE loginStatus = 'activo'");
      res.json(result.recordset[0]); 
    } catch (error) {
      console.error('Error al obtener usuarios conectados:', error);
      res.status(500).send('Error en el servidor');
    }
  });

//Partidas Jugadas Totales
app.get('/api/partidas/count', async (req, res) => {
    try {
      const result = await new sql.Request().query("SELECT COUNT(*) AS count FROM Partidas");
      res.json(result.recordset[0]);
    } catch (error) {
      console.error('Error al obtener el total de partidas:', error);
      res.status(500).send('Error en el servidor');
    }
  });

//Top 3 Usuarios
app.get('/api/usuarios/top', (req, res) => {
    const query = `
      SELECT U.nombre, COUNT(P.ID) AS partidas 
      FROM Usuarios U
      LEFT JOIN Partidas P ON U.[Pers.No.] = P.[Pers.No.]
      GROUP BY U.nombre
      ORDER BY partidas DESC
      OFFSET 0 ROWS FETCH NEXT 3 ROWS ONLY;`;
  
    new sql.Request().query(query, (err, result) => {
      if (err) {
        console.error('Error ejecutando la consulta:', err);
        res.status(500).send('Error en el servidor');
      } else {
        res.json(result.recordset); // Enviar el Top 3 como respuesta
      }
    });
  });

//Partidas jugadas hoy
app.get('/api/partidas/today', (req, res) => {
    const query = `
      SELECT COUNT(*) AS todayGames 
      FROM Partidas
    `;
    new sql.Request().query(query, (err, result) => {
      if (err) {
        console.error('Error al ejecutar la consulta:', err);
        res.status(500).send('Error en el servidor');
      } else {
        res.json({ todayGames: result.recordset[0].todayGames });
      }
    });
  });
  

// Usarios con logros
app.get('/api/usuarios/puntos-totales', async (req, res) => {
  try {
      const result = await new sql.Request().query(`
          SELECT 
              u.[Pers.No.], 
              u.nombre, 
              SUM(p.Puntaje) AS puntos_totales
          FROM dbo.Usuarios u
          JOIN dbo.Partidas p ON u.[Pers.No.] = p.[Pers.No.]
          GROUP BY u.[Pers.No.], u.nombre
          HAVING SUM(p.Puntaje) >= 1000;  -- Solo mostrar usuarios con más de 1000 puntos
      `);

      const usuarios = result.recordset;
      res.json({ usuarios });
  } catch (error) {
      console.error('Error al obtener los puntos totales:', error);
      res.status(500).send('Error en el servidor');
  }
});

//Usuarios con mas de 10 puntos
app.get('/api/usuarios/mas-de-10-puntos', async (req, res) => {
    try {
        const result = await new sql.Request().query(`
            SELECT u.[Pers.No.], u.nombre, SUM(p.Puntaje) AS puntos
            FROM dbo.Usuarios u
            JOIN dbo.Partidas p ON u.[Pers.No.] = p.[Pers.No.]
            GROUP BY u.[Pers.No.], u.nombre
            HAVING SUM(p.Puntaje) > 10;
        `);

        res.json({ usuarios: result.recordset });
    } catch (error) {
        console.error('Error al obtener usuarios con más de 10 puntos:', error);
        res.status(500).send('Error en el servidor');
    }
});

 //Usuarios con mayor puntaje 
app.get('/api/usuarios/mayor-puntaje', async (req, res) => {
    try {
        const result = await new sql.Request().query(`
            WITH MaxPuntaje AS (
                SELECT MAX(Puntaje) AS mayor_puntaje
                FROM dbo.Partidas
            )
            SELECT u.[Pers.No.], u.nombre, p.Puntaje
            FROM dbo.Usuarios u
            JOIN dbo.Partidas p ON u.[Pers.No.] = p.[Pers.No.]
            JOIN MaxPuntaje mp ON p.Puntaje = mp.mayor_puntaje;
        `);

        const usuarios = result.recordset;
        const cantidadUsuarios = usuarios.length;

        // Generar el mensaje dependiendo de la cantidad de usuarios con mayor puntaje
        let mensaje;
        if (cantidadUsuarios === 1) {
            mensaje = `1 usuario tiene el mayor puntaje.`;
        } else {
            mensaje = `${cantidadUsuarios} usuarios tienen el mayor puntaje.`;
        }

        res.json({
            mensaje,
            usuarios
        });
    } catch (error) {
        console.error('Error al obtener el usuario con mayor puntaje:', error);
        res.status(500).send('Error en el servidor');
    }
});

// Ruta para obtener el promedio de duración de las partidas
app.get('/api/partidas/promedio-duracion', async (req, res) => {
    try {
        const result = await new sql.Request().query(`
            SELECT AVG(Duracion) AS promedio_duracion
            FROM dbo.Partidas
        `);

        const promedio = result.recordset[0].promedio_duracion;

        res.json({ promedio });
    } catch (error) {
        console.error('Error al obtener el promedio de duración:', error);
        res.status(500).send('Error en el servidor');
    }
});

//Tomar datos para hacer login
app.post('/api/login', async (req, res) => {
  const { persNo, birthDate } = req.body;  // Recibimos el Pers.No. y la fecha de nacimiento desde el frontend

  // Validar que los datos sean correctos antes de continuar
  if (!persNo || !birthDate) {
      return res.status(400).json({ success: false, message: 'Faltan credenciales' });
  }

  try {
      // Query con variables enviadas por el frontend
      const query = `
          SELECT e.[Pers.No.], e.[Birth date]
          FROM dbo.EmpleadosKIA e
          WHERE e.[Pers.No.] = @persNo 
            AND e.[Birth date] = @birthDate;
      `;

      // Crear una nueva solicitud a la base de datos
      const request = new sql.Request();
      request.input('persNo', sql.NVarChar, persNo);    // Usar los datos ingresados por el usuario
      request.input('birthDate', sql.Date, birthDate);  // Formato de fecha compatible con SQL Server

      // Ejecutar la consulta
      const result = await request.query(query);

      // Verificar si el usuario fue encontrado
      if (result.recordset.length > 0) {
          // Verificar si es el administrador
          if (persNo === 'admin' && birthDate === '1234-12-12') {
              res.json({ success: true, role: 'admin', message: 'Login exitoso como admin' });
          } else {
              res.json({ success: true, role: 'user', message: 'Login exitoso como usuario' });
          }
      } else {
          res.status(401).json({ success: false, message: 'Credenciales incorrectas' });
      }
  } catch (error) {
      console.error('Error durante el login:', error);
      res.status(500).send('Error en el servidor');
  }
});

app.get('/api/usuarios/:persNo', async (req, res) => {
  const { persNo } = req.params;

  try {
    const query = `
      SELECT 
        e.[Pers.No.],
        u.nombre,
        e.[Employee Group] AS grupo,
        e.[Birth date] AS [Registro en],
        SUM(p.Duracion) AS TiempoTotalJugado,
        SUM(p.Puntaje) AS PuntajeTotal,
        COUNT(p.ID) AS PartidasTotales,
        CASE 
          WHEN SUM(p.Puntaje) >= 5000 THEN 5  
          ELSE FLOOR(SUM(p.Puntaje) / 1000)  
        END AS Logros,
        -- Obtener la puntuación de la última partida
        (SELECT TOP 1 p2.Puntaje 
         FROM dbo.Partidas p2 
         WHERE p2.[Pers.No.] = e.[Pers.No.]
         ORDER BY p2.ID DESC) AS UltimaPuntuacion
      FROM dbo.EmpleadosKIA e
      JOIN dbo.Usuarios u ON e.[Pers.No.] = u.[Pers.No.]
      LEFT JOIN dbo.Partidas p ON e.[Pers.No.] = p.[Pers.No.]
      WHERE e.[Pers.No.] = @persNo
      GROUP BY e.[Pers.No.], u.nombre, e.[Employee Group], e.[Birth date]
    `;

    const request = new sql.Request();
    request.input('persNo', sql.NVarChar, persNo);
    const result = await request.query(query);

    if (result.recordset.length > 0) {
      res.json(result.recordset[0]);
    } else {
      res.status(404).json({ message: 'No se encontraron datos del usuario.' });
    }
  } catch (error) {
    console.error('Error al obtener datos del usuario:', error);
    res.status(500).json({ message: 'Error en el servidor.' });
  }
});

app.post('/api/usersgame/logingame', async (req, res) => {
    const { username, password } = req.body;

    try {
        // Query to find the user
        const result = await sql.query`SELECT * FROM Usuarios WHERE nombre = ${username} AND Password = ${password}`;
        
        if (result.recordset.length > 0) {
            res.status(200).json({ message: 'Login successful!' });
        } else {
            res.status(401).json({ message: 'Invalid username or password' });
        }
    } catch (err) {
        console.error('Error during login:', err);
        res.status(500).json({ message: 'Internal server error' });
    }
});

// register.js
// Registration endpoint
// Registration endpoint
app.post('/api/usersgame/registergame', async (req, res) => {
    const { username, password, persNo } = req.body;

    // Check if required fields are provided
    if (!username || !password || !persNo) {
        return res.status(400).json({ message: 'Username, password, and Pers.No are required.' });
    }

    try {
        // Check if Pers.No exists in the EmpleadosKIA table
        const empleadosCheck = await sql.query`SELECT * FROM EmpleadosKIA WHERE [Pers.No.] = ${persNo}`;
        if (empleadosCheck.recordset.length === 0) {
            return res.status(404).json({ message: 'Pers.No does not exist in EmpleadosKIA.' });
        }

        // Check if the username already exists in the Users table
        const userCheck = await sql.query`SELECT * FROM Usuarios WHERE nombre = ${username}`;
        if (userCheck.recordset.length > 0) {
            return res.status(409).json({ message: 'Username already exists.' });
        }

        // Insert the new user with a default loginStatus of 'True'
        const insertUser = await sql.query`
            INSERT INTO Usuarios (nombre, password, [Pers.No.], loginStatus)
            VALUES (${username}, ${password}, ${persNo}, 'True')`;

        res.status(201).json({ message: 'User registered successfully.' });
    } catch (err) {
        console.error('Error during registration:', err);
        res.status(500).json({ message: 'Internal server error' });
    }
});

// Insert new data into the Partidas table using the registered Pers.No
app.post('/api/usersgame/partidasgame', async (req, res) => {
  const { nombre, duracion, puntaje } = req.body;

  if (!nombre || !duracion || !puntaje) {
    return res.status(400).json({ message: 'Nombre, Duracion, and Puntaje are required.' });
  }

  try {
    // Retrieve the Pers.No based on the provided nombre (username)
    const userResult = await sql.query`SELECT [Pers.No.] FROM Usuarios WHERE nombre = ${nombre}`;

    if (userResult.recordset.length === 0) {
      console.error(`User with nombre ${nombre} not found.`);
      return res.status(404).json({ message: 'User not found.' });
    }

    // Access the Pers.No. column using bracket notation
    const persNo = userResult.recordset[0]['Pers.No.']; 
    console.log(`Retrieved Pers.No: ${persNo} for nombre: ${nombre}`);

    // Insert the new partida data using the retrieved Pers.No
    await sql.query`
      INSERT INTO Partidas ([Pers.No.], Duracion, Puntaje, nombre)
      VALUES (${persNo}, ${duracion}, ${puntaje}, ${nombre})`;

    res.status(201).json({ message: 'Partida inserted successfully.' });
  } catch (err) {
    console.error('Error inserting partida:', err);
    res.status(500).json({ message: 'Internal server error' });
  }
});




// Sample endpoint to fetch users (optional)
app.get('/api/usersgame', async (req, res) => {
    try {
        const result = await sql.query`SELECT * FROM Usuarios`;
        res.status(200).json(result.recordset);
    } catch (err) {
        console.error('Error fetching users:', err);
        res.status(500).json({ message: 'Internal server error' });
    }
});



app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});
