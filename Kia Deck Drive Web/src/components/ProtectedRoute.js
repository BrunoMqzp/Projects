import React from 'react';
import { Navigate } from 'react-router-dom';

// Este componente recibe 'auth' como prop para verificar si el usuario está autenticado
const ProtectedRoute = ({ auth, children }) => {
  // Si el usuario no está autenticado, se le redirige a la página de login
  if (!auth) {
    return <Navigate to="/login" replace />;
  }

  // Si está autenticado, permite el acceso a la ruta protegida
  return children;
};

export default ProtectedRoute;
