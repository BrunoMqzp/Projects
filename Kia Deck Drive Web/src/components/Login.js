import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

const Login = ({ setAuth }) => {
  const [persNo, setPersNo] = useState("");
  const [birthDate, setBirthDate] = useState("");
  const navigate = useNavigate(); // Hook para redirigir programáticamente
  const [view, setView] = useState("login");

  const handleSubmit = async (e) => {
    e.preventDefault();
      
    try {
      const response = await fetch('http://localhost:5000/api/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ persNo, birthDate }),
      });
  
      const data = await response.json();
  
      if (data.success) {
        // Guarda el Pers.No. en localStorage
        localStorage.setItem('persNo', persNo);  // Cambia 'data.persNo' a 'persNo' si el backend no está enviando el Pers.No.
        
        setAuth({ isAuthenticated: true, role: data.role });
        
        if (data.role === 'admin') {
          navigate('/dashboard');
        } else if (data.role === 'user') {
          navigate('/home');
        }
      } else {
        alert("Credenciales incorrectas");
      }
    } catch (error) {
      alert("Error al conectar con el servidor");
    }
  };

  return (
    <div className="container-fluid">
      <div className="row min-vh-100 flex-center g-0">
        <div className="col-lg-8 col-xxl-5 py-3 position-relative">
          <img className="bg-auth-circle-shape" src="./assets/img/icons/spot-illustrations/bg-shape.png" alt="" width="250" />
          <img className="bg-auth-circle-shape-2" src="./assets/img/icons/spot-illustrations/shape-1.png" alt="" width="150" />
          <div className="card overflow-hidden z-1">
            <div className="row g-0 h-100">
            {/* Left Side */}
            <div className="col-md-5 text-center bg-card-gradient">
              <div className="position-relative p-4 pt-md-5 pb-md-7" data-bs-theme="light">
                <div className="bg-holder bg-auth-card-shape" style={{ backgroundImage: "url(./assets/img/icons/spot-illustrations/half-circle.png)" }}></div>
                <div className="z-1 position-relative">
                  <a className="link-light mb-4 font-sans-serif fs-5 d-inline-block fw-bolder" href="./index.html">Kia DD</a>
                  <p className="opacity-75 text-white">¡El juego de cartas sensación del momento! Desarrollado por estudiantes estrella del Tec de Monterrey.</p>
                </div>
              </div>
              <div className="mt-3 mb-4 mt-md-4 mb-md-5" data-bs-theme="light">
                <p className="text-white">
                  ¿No tienes una cuenta?
                  <br />
                  <a className="text-decoration-underline link-light" href="#!" onClick={() => setView("register")}>¡Registrémonos!</a>
                </p>
                <p className="mb-0 mt-4 mt-md-5 fs-10 fw-semi-bold text-white opacity-75">
                  Lée nuestros&nbsp;
                  <a className="text-decoration-underline text-white" href="#!">términos</a>
                  &nbsp;y&nbsp;
                  <a className="text-decoration-underline text-white" href="#!">condiciones</a>
                </p>
              </div>
            </div>

            {/* Right Side */}
            <div className="col-md-7 d-flex flex-center">
              <div className="p-4 p-md-5 flex-grow-1">
                {view === "login" && (
                  <>
                    <div className="row flex-between-center">
                      <div className="col-auto">
                        <h3>Inicio de sesión</h3>
                      </div>
                    </div>
                    <form onSubmit={handleSubmit}>
                      <div className="mb-3">
                        <label className="form-label" htmlFor="card-email">Número de empleado</label>
                        <input
                          type="text"
                          value={persNo}
                          className="form-control"
                          id="card-email"
                          onChange={(e) => setPersNo(e.target.value)}
                          placeholder="12345678"
                          required
                        />
                      </div>
                      <div className="mb-3">
                        <div className="d-flex justify-content-between">
                          <label className="form-label" htmlFor="dateInputmask">Fecha de nacimiento</label>
                        </div>
                        <input class="form-control" id="dateInputmask" data-input-mask='{"alias":"datetime","inputFormat":"yyyy/mm/dd"}' placeholder="AAAA-MM-DD" type="text" required value={birthDate} onChange={(e) => setBirthDate(e.target.value)} />
                      </div>
                      <div className="row flex-between-center">
                        <div className="col-auto">
                          <div className="form-check mb-0">
                            <input className="form-check-input" type="checkbox" id="card-checkbox" defaultChecked />
                            <label className="form-check-label mb-0" htmlFor="card-checkbox">Recuérdame</label>
                          </div>
                        </div>
                        <div className="col-auto">
                          <a className="fs-10" href="../../../pages/authentication/card/forgot-password.html">¿Olvidaste tus credenciales?</a>
                        </div>
                      </div>
                      <div className="mb-3">
                        <button className="btn btn-primary d-block w-100 mt-3" type="submit" name="submit">¡Vamos allá!</button>
                      </div>
                  </form>
              </>
          )}

                {view === "register" && (
                  <>
                    <div className="d-flex flex-center">
                      <div className="flex-grow-1">
                        <h3>Regístrate</h3>
                          <form>
                            <div className="mb-3">
                              <label className="form-label" htmlFor="card-name">Nombre</label>
                              <input className="form-control" type="text" autoComplete="on" id="card-name" />
                            </div>
                            <div className="mb-3">
                              <label className="form-label" htmlFor="card-email">Correo electrónico</label>
                              <input className="form-control" type="email" autoComplete="on" id="card-email" />
                            </div>
                            <div className="row gx-2">
                              <div className="mb-3 col-sm-6">
                                <label className="form-label" htmlFor="card-password">Contraseña</label>
                                <input className="form-control" type="password" autoComplete="on" id="card-password" />
                              </div>
                              <div className="mb-3 col-sm-6">
                                  <label className="form-label" htmlFor="card-confirm-password">Confirmar contraseña</label>
                                  <input className="form-control" type="password" autoComplete="on" id="card-confirm-password" />
                              </div>
                            </div>
                            <div className="form-check">
                              <input className="form-check-input" type="checkbox" id="card-register-checkbox" />
                              <label className="form-label" htmlFor="card-register-checkbox">
                                Acepto los
                                <a href="#!">&nbsp;términos </a>
                                y la
                                <a className="white-space-nowrap" href="#!"> política de privacidad</a>
                              </label>
                            </div>
                            <div className="mb-3">
                              <button className="btn btn-primary d-block w-100 mt-3" type="submit" name="submit">Regístrame</button>
                            </div>
                          </form>
                        </div>
                      </div>
                      <div className="mt-3">
                        <button className="btn btn-secondary d-block w-100" onClick={() => setView("login")}>Regresar al login</button>
                    </div>
                  </>
                    )}
                  </div>
                </div>
              </div>
          </div>
          <div className="text-center mt-3">
              <p className="mb-0">
                  <span className="d-none d-sm-inline-block">Desarrollado&nbsp;por estudiantes del Tec de Monterrey</span>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Login;