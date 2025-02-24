// App.js
import { BrowserRouter as Router, Route, Routes, Navigate, Link } from 'react-router-dom';
import Login from './Login';
import React, { useContext, useEffect, useState } from 'react';


function Dashboard() {
  const [userCount, setUserCount] = useState(null);
  const [connectedUsers, setConnectedUsers] = useState(null);  // Usuarios conectados
  const [registeredUsers, setRegisteredUsers] = useState(null);  // Usuarios registrados
  const [totalGames, setTotalGames] = useState(null);  // Partidas totales
  const [topUsers, setTopUsers] = useState([]);  // Top 3 usuarios
  const [todayGames, setTodayGames] = useState(null);
  const [topScorers, setTopScorers] = useState([]);
  const [mensajeTopScorers, setMensajeTopScorers] = useState('Cargando...');
  const [usuarios, setUsuarios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [usuariosConMasDe10Puntos, setUsuariosConMasDe10Puntos] = useState([]);
  const [loadingUsuarios10Puntos, setLoadingUsuarios10Puntos] = useState(true);
  const [promedioDuracion, setPromedioDuracion] = useState(null);
  const [loadingPromedio, setLoadingPromedio] = useState(true);


/*
  return (
    <Router>
      <Routes>
        {}
        <Route path="/dashboard" element={isAuthenticated ? <Dashboard /> : <Navigate to="/login" />} />
        {}
        <Route path="/login" element={<Login />} />
        {}
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </Router>
  );
*/


// Llamada a la API para obtener el número de usuarios usando fetch
useEffect(() => {
  const fetchUserCount = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/usuarios/count'); // URL de tu API
      const data = await response.json();
      setUserCount(data.count); // Guardar la cantidad de usuarios en el estado
    } catch (error) {
      console.error('Error al obtener el número de usuarios:', error);
    }
  };
  
  fetchUserCount(); // Ejecutar la función al montar el componente
}, []); // Solo se ejecuta una vez al montar el componente

// Fetch para obtener los usuarios conectados
useEffect(() => {
  const fetchConnectedUsers = async () => {
    try {
      const response = await fetch('http://localhost:5000/api/usuarios/conectados'); // Asegúrate de que esta ruta exista en tu backend
      const data = await response.json();
      setConnectedUsers(data.count); // Guardar el conteo de usuarios conectados
    } catch (error) {
      console.error('Error al obtener los usuarios conectados:', error);
    }
  };
  fetchConnectedUsers();
}, []);

  // Obtener partidas jugadas hoy
  useEffect(() => {
    const fetchTodayGames = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/partidas/hoy');
        const data = await response.json();
        setTodayGames(data.count);
      } catch (error) {
        console.error('Error al obtener partidas jugadas hoy:', error);
      }
    };
    fetchTodayGames();
  }, []);

  // Obtener usuarios registrados
  useEffect(() => {
    const fetchRegisteredUsers = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/usuarios/count');
        const data = await response.json();
        setRegisteredUsers(data.count);
      } catch (error) {
        console.error('Error al obtener usuarios registrados:', error);
      }
    };
    fetchRegisteredUsers();
  }, []);

  // Obtener partidas totales
  useEffect(() => {
    const fetchTotalGames = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/partidas/count');
        const data = await response.json();
        setTotalGames(data.count);
      } catch (error) {
        console.error('Error al obtener partidas totales:', error);
      }
    };
    fetchTotalGames();
  }, []);

  // Obtener top 3 usuarios
  useEffect(() => {
    const fetchTopUsers = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/usuarios/top');
        const data = await response.json();
        setTopUsers(data);
      } catch (error) {
        console.error('Error al obtener el top 3 de usuarios:', error);
      }
    };
    fetchTopUsers();
  }, []);

  //Juegos de hoy
  useEffect(() => {
    const fetchTodayGames = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/partidas/today');
        const data = await response.json();
        setTodayGames(data.todayGames);
      } catch (error) {
        console.error('Error al obtener las partidas de hoy:', error);
      }
    };
  
    fetchTodayGames();
  }, []);

  useEffect(() => {
    const fetchUsuarios = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/usuarios/puntos-totales');
            const data = await response.json();

            // Calcula los logros por cada usuario en el cliente
            const usuariosConLogros = data.usuarios.map(usuario => ({
                ...usuario,
                logros: Math.min(Math.floor(usuario.puntos_totales / 1000), 5) // Máximo 5 logros
            }));

            setUsuarios(usuariosConLogros);
            setLoading(false);
        } catch (error) {
            console.error('Error al obtener los usuarios:', error);
            setLoading(false);
        }
    };

    fetchUsuarios();
}, []);

useEffect(() => {
  const fetchUsuariosConMasDe10Puntos = async () => {
      try {
          const response = await fetch('http://localhost:5000/api/usuarios/mas-de-10-puntos');
          const data = await response.json();

          setUsuariosConMasDe10Puntos(data.usuarios);
          setLoadingUsuarios10Puntos(false);
      } catch (error) {
          console.error('Error al obtener los usuarios con más de 10 puntos:', error);
          setLoadingUsuarios10Puntos(false);
      }
  };

  fetchUsuariosConMasDe10Puntos();
}, []);
  

  useEffect(() => {
    const fetchTopScorers = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/usuarios/mayor-puntaje');
        const data = await response.json();
        
        setTopScorers(data.usuarios); // Guarda los usuarios en el estado
        setMensajeTopScorers(data.mensaje); // Guarda el mensaje recibido del backend
      } catch (error) {
        console.error('Error al obtener usuarios con mayor puntaje:', error);
        setMensajeTopScorers('Error al cargar los usuarios.');
      }
    };
  
    fetchTopScorers();
  }, []);

  useEffect(() => {
    const fetchPromedioDuracion = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/partidas/promedio-duracion');
            const data = await response.json();

            console.log('Promedio de duración:', data.promedio); // Verificar en consola
            setPromedioDuracion(data.promedio);
            setLoadingPromedio(false);
        } catch (error) {
            console.error('Error al obtener el promedio de duración:', error);
            setLoadingPromedio(false);
        }
    };

    fetchPromedioDuracion();
}, []);

  return (
    <>
      <main class="main" id="top">
        <nav class="navbar navbar-light navbar-glass navbar-top navbar-expand-lg">
          <a class="navbar-brand me-1 me-sm-3" href="../index.html">
            <div class="d-flex align-items-center">
              <img class="me-2" src="https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/KIA_logo3.svg/1024px-KIA_logo3.svg.png" alt="" width="40" /><span class="font-sans-serif text-primary">DriveDeck</span>
            </div>
          </a>
          <div class="collapse navbar-collapse scrollbar" id="navbarStandard">
            <ul class="navbar-nav" data-top-nav-dropdowns="data-top-nav-dropdowns">
            <li class="nav-item dropdown"><Link to="./login" class="nav-link" href="#" role="button" id="documentations">Cerrar sesión</Link>
                <div class="dropdown-menu dropdown-caret dropdown-menu-card border-0 mt-0" aria-labelledby="documentations">
                </div>
              </li>
            </ul>
          </div>
        </nav>
        <div class="content">
          <div class="container-xl">
            <div class="row g-3 mb-3">
              <div class="col-12">
                <div class="card bg-transparent-50 overflow-hidden">
                  <div class="card-header position-relative">
                    <div class="bg-holder d-none d-md-block bg-card z-1" style={{ backgroundImage: 'url(../assets/img/illustrations/ecommerce-bg.png)', backgroundSize: '230px', backgroundPosition: 'right bottom', zIndex: -1 }}>
                    </div>
                    <div class="position-relative z-2">
                      <div>
                        <h3 class="text-primary mb-1">¡Bienvenido, administrador!</h3>
                        <p>Te damos un resumen del día de hoy. </p>
                      </div>
                      <div class="d-flex py-3">
                        <div class="pe-3">
                          <p class="text-600 fs-10 fw-medium">Usuarios conectados </p>
                          <h4 className="text-800 mb-0">
                            {connectedUsers !== null ? `${connectedUsers} usuarios` : 'Cargando...'}
                            </h4>

                        </div>
                        <div className="ps-3">
                          <p className="text-600 fs-10">
                            Partidas jugadas hoy {new Date().toLocaleDateString('es-ES', { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' })}
                            </p>
                            <h4 className="text-800 mb-0">
                              {todayGames !== null ? `${todayGames} partidas` : 'Cargando...'}
                              </h4>
                              </div>

                      </div>
                    </div>
                  </div>
                  <div class="card-body p-0">
                    <ul class="mb-0 list-unstyled list-group font-sans-serif">
                      <li class="list-group-item mb-0 rounded-0 py-3 px-x1 list-group-item-warning border-x-0 border-top-0">
                        <div class="row flex-between-center">
                          <div class="col">
                            <div class="d-flex">
                              
                            <div className="card">
                              <div className="card-header">
                                <h6>Usuarios con logros</h6>
                                </div>
                                <div className="card-body">
                                  {loading ? (
                                    <p>Cargando logros...</p>
                                  ) : (
                                    <ul>
                                      {usuarios.map(usuario => (
                                        <li key={usuario['Pers.No.']}>
                                          {usuario.nombre}: {usuario.logros} logro(s)
                                        </li>
                                      ))}
                                    </ul>
                                  )}
                                </div>
                              </div>


                            </div>
                          </div>
                          <div class="col-auto d-flex align-items-center"><a class="fs-10 fw-medium text-warning-emphasis" href="#!">Ver más &rsaquo;<i class="fas fa-chevron-right ms-1 fs-11"></i> </a></div>
                        </div>
                      </li>
                      <li class="list-group-item mb-0 rounded-0 py-3 px-x1 greetings-item text-700 border-x-0 border-top-0">
                        <div class="row flex-between-center">
                          <div class="col">
                            <div class="d-flex">
                            <div className="card">
                              <div className="card-header">
                                <h6>Usuarios con más de 10 puntos</h6>
                                </div>
                                <div className="card-body">
                                  {loadingUsuarios10Puntos ? (
                                    <p>Cargando usuarios...</p>
                                  ) : (
                                    <ul>
                                      {usuariosConMasDe10Puntos.map(usuario => (
                                        <li key={usuario['Pers.No.']}>
                                          {usuario.nombre}: {usuario.puntos} puntos
                                        </li>
                                      ))}
                                    </ul>
                                  )}
                                </div>
                              </div>

                            </div>
                          </div>
                          <div class="col-auto d-flex align-items-center"><a class="fs-10 fw-medium" href="#!">Ver más &rsaquo;<i class="fas fa-chevron-right ms-1 fs-11"></i> </a></div>
                        </div>
                      </li>
                      <li class="list-group-item mb-0 rounded-0 py-3 px-x1 greetings-item text-700  border-0">
                        <div class="row flex-between-center">
                          <div class="col">
                            <div className="col-md-6 col-xxl-3">
                              <div className="card h-md-100">
                                <div className="card-header d-flex flex-between-center pb-0">
                                  <h6 className="mb-0">Usuarios con Mayor Puntaje</h6>
                                  </div>
                                  <div className="card-body">
                                    <p className="font-sans-serif">{mensajeTopScorers}</p>
                                    <ul className="list-unstyled">
                                      {topScorers.length > 0 ? (
                                        topScorers.map((usuario, index) => (
                                        <li key={index} className="d-flex justify-content-between">
                                          <span>{usuario.nombre}</span>
                                          <span>{usuario.Puntaje} puntos</span>
                                          </li>
                                        ))
                                      ) : (
                                        <li>No hay usuarios con puntaje registrado.</li>
                                      )}
                                    </ul>
                                  </div>
                                </div>
                              </div>

                          </div>
                          <div class="col-auto d-flex align-items-center"><a class="fs-10 fw-medium" href="#!">Ver más &rsaquo;<i class="fas fa-chevron-right ms-1 fs-11"></i> </a></div>
                        </div>
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
            <div class="row g-3 mb-3">
              <div class="col-md-6 col-xxl-3">
                <div class="card h-md-100">
                <div class="bg-holder bg-card" style={{ backgroundImage: 'url(https://prium.github.io/falcon/v3.22.0/assets/img/icons/spot-illustrations/corner-1.png)' }}>
                </div>
                  <div class="card-header pb-0">
                    <h6 class="mb-0">Promedio de duración de partidas</h6>
                  </div>
                  <div class="card-body d-flex flex-column justify-content-end">
                    <div className="row">
                      <div className="col">
                        <p className="font-sans-serif lh-1 mb-1 fs-5">
                          {loadingPromedio
                              ? 'Cargando...'
                              : `${Math.round(promedioDuracion)} minutos`}
                        </p>
                        <span className="badge badge-subtle-success rounded-pill fs-11">
                          +0.0%
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div className="col-md-6 col-xxl-3">
                <div class="card h-md-100 ecommerce-card-min-width">
              <div class="bg-holder bg-card" style={{ backgroundImage: 'url(https://prium.github.io/falcon/v3.22.0/assets/img/icons/spot-illustrations/corner-2.png)' }}>
              </div>
                  <div class="card-header pb-0">
                    <h6 class="mb-0">Usuarios registrados</h6>
                  </div>
                  <div class="card-body d-flex flex-column justify-content-end">
                    <div class="row">
                      <div class="col">
                        {/* Mostrar el número de usuarios registrado dinámicamente */}
                        <p class="font-sans-serif lh-1 mb-1 fs-5">
                        {userCount !== null ? userCount : 'Cargando...'}
                        </p>
                        <span class="badge badge-subtle-success rounded-pill fs-11">+0.0%</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="col-md-6 col-xxl-3">
                <div class="card h-md-100">
                <div class="bg-holder bg-card" style={{ backgroundImage: 'url(https://prium.github.io/falcon/v3.22.0/assets/img/icons/spot-illustrations/corner-4.png)' }}>
                </div>
                  <div class="card-body">
                    <div class="row h-100 justify-content-between g-0">
                      <div class="col-5 col-sm-6 col-xxl pe-2">
                        <h6 class="mt-1">Top 3 de usuarios</h6>
                        <div className="fs-11 mt-3">
                          {topUsers !== null ? (topUsers.map((user, index) => (
                            <div className="d-flex flex-between-center mb-1" key={index}>
                              <div className="d-flex align-items-center">
                                <span className={`dot bg-${index === 0 ? 'primary' : index === 1 ? 'info' : '300'}`}></span>
                                <span className="fw-semi-bold">{user.nombre}</span>
                                </div>
                                <div className="d-xxl-none">{user.partidas} partidas</div>
                              </div>
                            ))
                          ) : (
                            <p>Cargando...</p>
                          )}
                        </div>

                        <div class="fs-11 mt-3">
                        </div>
                      </div>
                      <div class="col-auto position-relative">
                        <div class="text-1100 fs-7">26M</div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
              <div className="col-md-6 col-xxl-3">
  <div className="card h-md-100">
  <div class="bg-holder bg-card" style={{ backgroundImage: 'url(https://prium.github.io/falcon/v3.22.0/assets/img/icons/spot-illustrations/corner-5.png)' }}>
  </div>
    <div className="card-header d-flex flex-between-center pb-0">
      <h6 className="mb-0">Todas las partidas jugadas</h6>
    </div>
    <div className="card-body pt-2">
      <div className="row g-0 h-100 align-items-center justify-content-center">
        <div className="col-auto text-center ps-2">
          <div
            className="fs-5 fw-normal font-sans-serif text-primary mb-1 lh-1"
            style={{
              fontSize: '5rem',
              fontWeight: 'normal',
              fontFamily: 'sans-serif',
              color: 'primary',
              marginBottom: '1rem',
              lineHeight: '1',
            }}
          >
            {totalGames !== null ? `${totalGames} partidas` : 'Cargando...'}
          </div>
        </div>
      </div>
    </div>
  </div>
</div>

            </div>
            <div class="card">
                <div class="bg-holder bg-card" style={{ backgroundImage: 'url(https://prium.github.io/falcon/v3.22.0/assets/img/icons/spot-illustrations/corner-4.png)' }}>
                </div>

                <div class="card-body position-relative">
                  <div class="row g-2 align-items-sm-center">
                    <div class="col-auto"><img src="../assets/img/icons/connect-circle.png" alt="" height="55" /></div>
                    <div class="col">
                      <div class="row align-items-center">
                        <div class="col col-lg-8">
                          <h5 class="fs-9 mb-3 mb-sm-0 text-primary">Conéctate a la base de datos aquí</h5>
                        </div>
                        <div class="col-12 col-sm-auto ms-auto">
                        <Link to="/crud" class="btn btn-falcon-primary" type="button">Conectar</Link>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          </div>
          <footer class="footer">
            <div class="row g-0 justify-content-between fs-10 mt-4 mb-3">
              <div class="col-12 col-sm-auto text-center">
                <p class="mb-0 text-600">Kia Drive Deck <span class="d-none d-sm-inline-block">|&nbsp;</span>2024 © <a href="#">Equipo 4</a></p>
              </div>
              <div class="col-12 col-sm-auto text-center">
                <p class="mb-0 text-600">v1.0</p>
              </div>
            </div>
          </footer>
        </div>
      </main>
    </>
  );
}
export default Dashboard;