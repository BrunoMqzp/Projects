import React from "react";
import { Link } from "react-router-dom";

const Home = () => {
  return (
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
      <div className="container">
      <div className="row g-3 mb-3">
            <div className="col-xxl-6">
              <div className="row g-3">
                <div className="col-12">
                  <div className="card font-sans-serif">
                    <div className="card-body d-flex gap-3 flex-column flex-sm-row align-items-center"><img className="rounded-3" src="https://static-00.iconduck.com/assets.00/user-icon-512x512-x23sj495.png" alt="" width="112" />
                      <table className="table table-borderless fs-10 fw-medium mb-0">
                        <tbody>
                          <tr>
                            <td className="p-1" StyleName={{ width: '35%' }}>Nombre:</td> { /* ACÁ VA EL NOMBRE DEL USUARIO */ }
                            <td className="p-1 text-600">Diego Garza Montemayor</td>
                          </tr>
                          <tr>
                            <td className="p-1" StyleName={{ width: '35%' }}>Registro en:</td> { /* ACÁ VA EL REGISTRO DE LA FECHA DE INICIO DE SESIÓN */ }
                            <td className="p-1 text-600">2021/01/12</td>
                          </tr>
                          <tr>
                            <td className="p-1" StyleName={{ width: '35%' }}>Grupo:</td> { /* ACÁ VA EL EMAIL DEL USUARIO */ }
                            <td className="p-1"><a className="text-600 text-decoration-none">Sindicalizado </a><small className="badge rounded badge-subtle-success false">Verificado</small></td>
                          </tr>
                        </tbody>
                      </table>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="card font-sans-serif">
                    <div className="card-header pb-0">
                      <h6 className="mb-0">Tiempo total jugado</h6>
                    </div>
                    <div className="card-body">
                      <div className="row g-0">
                        <div className="col-6"> {/* CAMBIAR POR EL QUE DIGA EN LA BD */}
                          <h4 className="text-700 lh-1 mb-1"> 0</h4><small className="badge rounded badge-subtle-warning false">Histórico</small>
                        </div>
                        <div className="col-6 mt-n4 d-flex justify-content-end">
                          <div className="echart-default" data-echart-responsive="true" data-echarts="{&quot;xAxis&quot;:{&quot;data&quot;:[&quot;Day 1&quot;,&quot;Day 2&quot;,&quot;Day 3&quot;,&quot;Day 4&quot;,&quot;Day 5&quot;,&quot;Day 6&quot;,&quot;Day 7&quot;,&quot;Day 8&quot;,&quot;Day 9&quot;,&quot;Day 10&quot;]},&quot;series&quot;:[{&quot;type&quot;:&quot;line&quot;,&quot;data&quot;:[85,60,120,70,100,15,65,80,60,75,45],&quot;smooth&quot;:true,&quot;lineStyle&quot;:{&quot;width&quot;:2}}],&quot;grid&quot;:{&quot;bottom&quot;:&quot;2%&quot;,&quot;top&quot;:&quot;2%&quot;,&quot;right&quot;:&quot;0px&quot;,&quot;left&quot;:&quot;0px&quot;}}" _echarts_instance_="ec_1729056767463" StyleName="-webkit-user-select: none; position: relative;"><div StyleName="position: relative; width: 124px; height: 80px; padding: 0px; margin: 0px; border-width: 0px; cursor: pointer;"><canvas data-zr-dom-id="zr_0" width="124" height="80" StyleName="position: absolute; left: 0px; top: 0px; width: 124px; height: 80px; -webkit-user-select: none; padding: 0px; margin: 0px; border-width: 0px;"></canvas></div><div className=""></div></div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="card font-sans-serif">
                    <div className="card-header pb-0">
                      <h6 className="mb-0">Puntaje total</h6>
                    </div>
                    <div className="card-body">
                      <div className="row g-0">
                        <div className="col-6"> { /* CAMBIAR POR EL QUE DIGA LA BASE DE DATOS */ }
                          <h4 className="text-700 lh-1 mb-1"> 0</h4><small className="badge rounded badge-subtle-warning false">Histórico</small> { /* ACÁ CAMBIAR EL MES POR EL ACTUAL (EJ. OCT, NOV, DIC) */}
                        </div>
                        <div className="col-6 mt-n4 d-flex justify-content-end">
                          <div className="echart-default" data-echart-responsive="true" data-echarts="{&quot;xAxis&quot;:{&quot;data&quot;:[&quot;Day 1&quot;,&quot;Day 2&quot;,&quot;Day 3&quot;,&quot;Day 4&quot;,&quot;Day 5&quot;,&quot;Day 6&quot;,&quot;Day 7&quot;,&quot;Day 8&quot;]},&quot;series&quot;:[{&quot;type&quot;:&quot;line&quot;,&quot;data&quot;:[55,60,40,120,70,80,35,80,85],&quot;smooth&quot;:true,&quot;lineStyle&quot;:{&quot;width&quot;:2}}],&quot;grid&quot;:{&quot;bottom&quot;:&quot;2%&quot;,&quot;top&quot;:&quot;2%&quot;,&quot;right&quot;:&quot;0px&quot;,&quot;left&quot;:&quot;10px&quot;}}" _echarts_instance_="ec_1729056767464" StyleName="-webkit-user-select: none; position: relative;"><div StyleName="position: relative; width: 124px; height: 80px; padding: 0px; margin: 0px; border-width: 0px; cursor: pointer;"><canvas data-zr-dom-id="zr_0" width="124" height="80" StyleName="position: absolute; left: 0px; top: 0px; width: 124px; height: 80px; -webkit-user-select: none; padding: 0px; margin: 0px; border-width: 0px;"></canvas></div><div className=""></div></div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="col-xxl-6">
              <div className="card h-100 font-sans-serif">
                <div className="card-header bg-body-tertiary d-flex flex-between-center py-2">
                  <h6 className="mb-0">Estadísticas</h6><a className="btn btn-link btn-sm px-0 fw-medium" href="#!">Ver más &rsaquo;</a>
                </div>
                <div className="card-body">
                  <div className="row g-0 h-100">
                    <div className="col-sm-7 order-1 order-sm-0">
                      <div className="row g-sm-0 gy-4 row-cols-2 h-100 align-content-between">
                        <div className="col">
                          <div className="d-flex gap-2 mb-3">
                            <div className="vr rounded ps-1 bg-success"></div>
                            <h6 className="lh-base text-700 mb-0">ID de Empleado</h6>
                          </div>
                          <h5 className="fw-normal"> 0</h5>
                          <h6 className="mb-0"><span className="text-500 me-2">esta semana</span><small className="badge rounded badge-subtle-success false">2.1%</small></h6>
                        </div>
                        <div className="col">
                          <div className="d-flex gap-2 mb-3">
                            <div className="vr rounded ps-1 bg-primary"></div>
                            <h6 className="lh-base text-700 mb-0">Datos de las últimas partidas</h6>
                          </div>
                          <h5 className="fw-normal"> 0</h5>
                          <h6 className="mb-0"><span className="text-500 me-2">esta semana</span><small className="badge rounded badge-subtle-danger false">5.1%</small></h6>
                        </div>
                        <div className="col">
                          <div className="d-flex gap-2 mb-3">
                            <div className="vr rounded ps-1 bg-info"></div>
                            <h6 className="lh-base text-700 mb-0">Partidas totales</h6>
                          </div>
                          <h5 className="fw-normal"> 0</h5>
                          <h6 className="mb-0"><span className="text-500 me-2">esta semana</span><small className="badge rounded badge-subtle-secondary dark__bg-1000">0.0%</small></h6>
                        </div>
                        <div className="col">
                          <div className="d-flex gap-2 mb-3">
                            <div className="vr rounded ps-1 bg-warning"></div>
                            <h6 className="lh-base text-700 mb-0">Logros</h6>
                          </div>
                          <h5 className="fw-normal"> 0</h5>
                          <h6 className="mb-0"><span className="text-500 me-2">esta semana</span><small className="badge rounded badge-subtle-primary false">3.5%</small></h6>
                        </div>
                      </div>
                    </div>
                    <div className="col-sm-5 mb-5 mb-sm-0">
                      <div className="echart-assignment-scores" data-echart-responsive="true" _echarts_instance_="ec_1729056767486" StyleName="-webkit-user-select: none; position: relative;"><div StyleName="position: relative; width: 230px; height: 200px; padding: 0px; margin: 0px; border-width: 0px; cursor: default;"><canvas data-zr-dom-id="zr_0" width="230" height="200" StyleName="position: absolute; left: 0px; top: 0px; width: 230px; height: 200px; -webkit-user-select: none; padding: 0px; margin: 0px; border-width: 0px;"></canvas></div><div className=""></div></div>
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
                          <h5 class="fs-9 mb-3 mb-sm-0 text-primary">¡Vamos a jugar!</h5>
                        </div>
                        <div class="col-12 col-sm-auto ms-auto">
                        <Link to="/juego" class="btn btn-falcon-primary" type="button">Correr juego</Link>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          </div>
        </div>
        </div>
    </main>
  );
};

export default Home;