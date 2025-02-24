import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate, Link } from 'react-router-dom';

function Crud() {
  const [users, setUsers] = useState([
    { id: 1, name: "John Doe", email: "john@example.com" },
    { id: 2, name: "Jane Smith", email: "jane@example.com" }
  ]);
  const [newUser, setNewUser] = useState({ name: '', email: '' });
  const [editingUser, setEditingUser] = useState(null);
  const [editMode, setEditMode] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setNewUser((prevState) => ({ ...prevState, [name]: value }));
  };

  const handleCreate = (e) => {
    e.preventDefault();
    setUsers([...users, { id: users.length + 1, ...newUser }]);
    setNewUser({ name: '', email: '' });
  };

  const handleEdit = (user) => {
    setEditMode(true);
    setEditingUser(user);
    setNewUser(user);
  };

  const handleUpdate = (e) => {
    e.preventDefault();
    setUsers(users.map((u) => (u.id === editingUser.id ? newUser : u)));
    setEditMode(false);
    setNewUser({ name: '', email: '' });
    setEditingUser(null);
  };

  const handleDelete = (userId) => {
    setUsers(users.filter((user) => user.id !== userId));
  };

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
                    <h3 className="mb-0">Adminstración de usuarios</h3>
                  </div>
                  <div class="col-12 col-sm-auto ms-auto">
                  <Link to="/dashboard" class="btn btn-falcon-primary" type="button">Regresar</Link>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="card mb-3">
          <div className="card-header">
            <h5 className="mb-0" data-anchor="data-anchor" id="quick-start">Sistema de gestión troncal<a className="anchorjs-link " aria-label="Anchor" data-anchorjs-icon="#" href="#quick-start" style={{ marginLeft: '0.1875em', paddingRight: '0.1875em', paddingLeft: '0.1875em' }}></a></h5>
          </div>
          <div className="card-body bg-body-tertiary">
            <p className="mb-0">En conjunto, las operaciones <a class="text-warning">CRUD</a> facilitan la gestión eficiente de los datos en una base de datos, asegurando que la información pueda ser añadida, accedida, modificada y eliminada según sea necesario.</p>
          </div>
        </div>
          {/* TABLA TRONCAL */}
          <h1 className="text-center mb-3 mt-5">TABLA TRONCAL<code>SQL</code></h1>

          {/* User Form */}
          <div className="card mb-3">
            <div className='card-header'>
            <h5 className="mb-0">{editMode ? 'Editar usuario' : 'Añadir usuario'}</h5>
            </div>
            <div className="card-body bg-body-tertiary">
              <p>Por favor, rellena los campos para añadir un nuevo usuario.</p>
              <form onSubmit={editMode ? handleUpdate : handleCreate}>
                <div className="row mb-3">
                  <div className="col col-sm-6 mb-3">
                  <label htmlFor="name" className="form-label">Nombre del usuario</label>
                  <input
                      type="text"
                      className="form-control"
                      id="name"
                      name="name"
                      value={newUser.name}
                      onChange={handleChange}
                      required
                  />
                  </div>
                  <div className="col col-sm-6 mb-3">
                  <label htmlFor="email" className="form-label">Correo electrónico</label>
                  <input
                      type="email"
                      className="form-control"
                      id="email"
                      name="email"
                      value={newUser.email}
                      onChange={handleChange}
                      required
                  />
                  </div>
                </div>
                <button type="submit" className="btn btn-primary">
                {editMode ? 'Actualizar fila' : 'Añadir usuario'}
                </button>
                {editMode && (
                <button
                    type="button"
                    className="btn btn-secondary ms-2"
                    onClick={() => {
                    setEditMode(false);
                    setNewUser({ name: '', email: '' });
                    }}
                >
                    Cancelar
                </button>
                )}
              </form>
          </div>
        </div>

        {/* User List */}
        <div className='mt-3 mb-5 card'> 
          <div className='card-header'>
          <h5 className="mb-0">Lista de usuarios</h5>
          </div>
          <div className="card-body bg-body-tertiary">
          <div class="alert alert-warning d-flex align-items-center" role="alert">
            <p class="mb-0 flex-1">Las acciones realizadas en la tabla son <strong>irreversibles</strong>. ¡Proceder con cautela!</p><button class="btn-close" type="button" data-bs-dismiss="alert" aria-label="Close"></button>
          </div>
          <table className="table table-bordered mt-3">
              <thead>
              <tr>
                  <th>ID</th>
                  <th>Nombre</th>
                  <th>Correo electrónico</th>
                  <th>Acciones disponibles</th>
              </tr>
              </thead>
              <tbody>
              {users.map((user) => (
                  <tr key={user.id}>
                  <td>{user.id}</td>
                  <td>{user.name}</td>
                  <td>{user.email}</td>
                  <td>
                      <button
                      className="btn btn-warning me-2"
                      onClick={() => handleEdit(user)}
                      >
                      Editar
                      </button>
                      <button
                      className="btn btn-danger"
                      onClick={() => handleDelete(user.id)}
                      >
                      Eliminar
                      </button>
                  </td>
                  </tr>
              ))}
              </tbody>
          </table>
        </div>
        </div>
      </div>
    </main>
  );
}

export default Crud;