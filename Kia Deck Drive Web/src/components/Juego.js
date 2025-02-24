// SamplePage.tsx
import React from "react";
import { Link } from "react-router-dom";

const Juego = () => {

  return (
    <main>
      <div className="container" data-layout="container">
        <div class="card mb-3 mt-5">
          <div class="bg-holder bg-card" style={{ backgroundImage: 'url(https://prium.github.io/falcon/v3.22.0/assets/img/icons/spot-illustrations/corner-1.png)' }}>
          </div>

          <div class="card-body position-relative">
            <div class="row g-2 align-items-sm-center">
              <div class="col-auto"><img src="../assets/img/icons/connect-circle.png" alt="" height="55" /></div>
              <div class="col">
                <div class="row align-items-center">
                  <div class="col col-lg-8">
                    <h3 className="mb-0">Kia Drive Deck</h3>
                  </div>
                  <div class="col-12 col-sm-auto ms-auto">
                  <Link to="/home" class="btn btn-falcon-primary" type="button">Salir del juego</Link>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    <div>
      <iframe

        title="Unity Game"
        className="mt-3"

        src="http://localhost:3001"

        style={{ width: '100%', height: '600px', border: 'none' }}

      ></iframe>

    </div>
    </main>

  );

};

export default Juego;
