import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import Login from "./components/Login";
import Crud from "./components/Crud";
import Home from "./components/Home";
import ProtectedRoute from "./components/ProtectedRoute";
import Dashboard from "./components/Dashboard";
import Juego from "./components/Juego";


const App = () => {
  const [auth, setAuth] = useState({
    isAuthenticated: false,
    role: null,
    persNo: null
  });

  // Cargar el estado de autenticación desde localStorage al montar el componente
  useEffect(() => {
    const storedAuth = localStorage.getItem('auth');
    if (storedAuth) {
      setAuth(JSON.parse(storedAuth));
    }
  }, []);

  // Guardar el estado de autenticación en localStorage cada vez que cambia
  useEffect(() => {
    localStorage.setItem('auth', JSON.stringify(auth));
  }, [auth]);

  return (
    <Router>
      <Routes>
        <Route
          path="/home"
          element={
            <ProtectedRoute auth={auth.isAuthenticated}>
              <Home auth={auth} />
            </ProtectedRoute>
          }
        />
        <Route
          path="/juego"
          element={
            <ProtectedRoute auth={auth.isAuthenticated}>
              <Juego />
            </ProtectedRoute>
          }
        />
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute auth={auth.isAuthenticated && auth.role === 'admin'}>
              <Dashboard />
            </ProtectedRoute>
          }
        />
        <Route
          path="/crud"
          element={
            <ProtectedRoute auth={auth.isAuthenticated && auth.role === 'admin'}>
              <Crud />
            </ProtectedRoute>
          }
        />
        <Route
          path="/login"
          element={<Login setAuth={setAuth} />}
        />
        <Route path="*" element={<Navigate to="/login" />} /> {/* Redirección por defecto */}
      </Routes>
    </Router>
  );
};

export default App;
