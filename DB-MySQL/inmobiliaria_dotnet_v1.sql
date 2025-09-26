-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 27-09-2025 a las 00:21:25
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria_dotnet_v1`
--
CREATE DATABASE IF NOT EXISTS `inmobiliaria_dotnet_v1` DEFAULT CHARACTER SET utf8 COLLATE utf8_spanish2_ci;
USE `inmobiliaria_dotnet_v1`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `idContrato` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `idInquilino` int(11) NOT NULL,
  `fechaInicio` date NOT NULL,
  `fechaFin` date NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `idUsuarioAlta` int(11) NOT NULL,
  `fechaBaja` date DEFAULT NULL,
  `idUsuarioBaja` int(11) DEFAULT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`idContrato`, `idInmueble`, `idInquilino`, `fechaInicio`, `fechaFin`, `precio`, `idUsuarioAlta`, `fechaBaja`, `idUsuarioBaja`, `estado`) VALUES
(1, 1, 1, '2025-03-01', '2026-11-29', 300000.00, 1, NULL, NULL, 1),
(2, 1, 3, '2025-10-05', '2025-12-06', 250000.00, 2, NULL, NULL, 1),
(3, 3, 2, '2025-09-18', '2025-09-27', 350000.00, 1, NULL, NULL, 1),
(4, 2, 4, '2025-09-26', '2025-09-29', 250000.00, 2, NULL, NULL, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `idInmueble` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `idPropietario` int(11) NOT NULL,
  `direccion` varchar(100) NOT NULL,
  `uso` varchar(30) NOT NULL,
  `tipo` varchar(100) NOT NULL,
  `cantAmbientes` int(11) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `habilitado` tinyint(1) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`idInmueble`, `nombre`, `idPropietario`, `direccion`, `uso`, `tipo`, `cantAmbientes`, `precio`, `habilitado`, `estado`) VALUES
(1, 'Casa San Juan I', 1, 'San Juan 37', 'Residencial', 'Casa con patio y pileta', 2, 280000.00, 1, 1),
(2, 'Casa San Juan II', 1, 'San Juan 55', 'Residencial', 'Dpto 1 dormitorio', 2, 250000.00, 1, 1),
(3, 'Cabaña El Coplero', 4, 'Poeta Conti 582', 'Residencial', 'Casa con patio y pileta', 3, 350000.00, 0, 1),
(4, 'Oficina N° 1 - GómezSA', 8, 'Calle Comerio 853', 'Comercial', 'Oficina comercial', 1, 320000.00, 1, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `idInquilino` int(11) NOT NULL,
  `dni` int(10) NOT NULL,
  `nombre` varchar(30) NOT NULL,
  `apellido` varchar(30) NOT NULL,
  `telefono` varchar(10) NOT NULL,
  `email` varchar(50) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`idInquilino`, `dni`, `nombre`, `apellido`, `telefono`, `email`, `estado`) VALUES
(1, 12345678, 'Inqui', 'Lino', '1234567899', 'inquilino@mail.com', 1),
(2, 34561878, 'José', 'Ferreyra', '2664123432', 'jferreyra@gmail.com', 1),
(3, 30222555, 'Mario', 'Cuenca', '3515456255', 'mario@cuenca.com', 1),
(4, 25489633, 'Morena', 'Ortíz', '2664562121', 'more@correo.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `idPago` int(11) NOT NULL,
  `idContrato` int(11) NOT NULL,
  `numero` int(11) NOT NULL,
  `fecha` date NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `detalle` varchar(100) NOT NULL,
  `idUsuarioAlta` int(11) NOT NULL,
  `idUsuarioBaja` int(11) DEFAULT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`idPago`, `idContrato`, `numero`, `fecha`, `precio`, `detalle`, `idUsuarioAlta`, `idUsuarioBaja`, `estado`) VALUES
(1, 1, 1, '2025-03-03', 300000.00, 'Marzo/25', 2, NULL, 1),
(2, 1, 2, '2025-04-04', 300000.00, 'Abril/25', 2, NULL, 1),
(3, 4, 1, '2025-09-26', 250000.00, 'Alquiler temporario', 2, NULL, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `idPropietario` int(11) NOT NULL,
  `dni` int(10) NOT NULL,
  `nombre` varchar(30) NOT NULL,
  `apellido` varchar(30) NOT NULL,
  `telefono` varchar(10) NOT NULL,
  `email` varchar(50) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`idPropietario`, `dni`, `nombre`, `apellido`, `telefono`, `email`, `estado`) VALUES
(1, 12312378, 'Juan', 'Pérez', '3515456789', 'juan.p@mail.com', 1),
(2, 30987654, 'Maria', 'García', '2664123456', 'm_garcia@gmail.com', 1),
(3, 15678902, 'Pedro', 'López', '1165678901', 'pedrolopez@yahoo.com', 1),
(4, 35432109, 'Ana', 'Rodríguez', '2235987654', 'ana.r78@hotmail.com', 1),
(5, 22345678, 'Sofía', 'Torres', '3416789012', 'st_arg@outlook.com.ar', 1),
(6, 38765432, 'Carlos', 'Fernández', '2617890123', 'carlosf12@gmail.com', 1),
(7, 19876543, 'Laura', 'Castro', '2998765432', 'laura.c@live.com', 1),
(8, 28567890, 'Diego', 'Gómez', '3875678901', 'diegog_85@protonmail.com', 1),
(9, 33210987, 'Florencia', 'Díaz', '2644567890', 'florencia.diaz@empresa.com.ar', 1),
(10, 12098765, 'Martín', 'Ruiz', '2915890123', 'ruiz.m@correo.net', 1),
(11, 12345000, 'Antonio', 'Manzanelli', '3515456789', 'aj@gmail.com', 1),
(12, 1234567, 'Maria', 'López', '1234567890', 'ml@gmail.com', 1),
(13, 55333444, 'Natalia', 'Urizar', '3515456001', 'nt@mail.com', 1),
(14, 34568431, 'Hernán', 'Mansilla', '3515354008', 'correo@mail.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `idTipoInmueble` int(11) NOT NULL,
  `descripcion` varchar(100) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`idTipoInmueble`, `descripcion`, `estado`) VALUES
(1, 'Monoambiente', 1),
(2, 'Dpto 1 dormitorio', 1),
(3, 'Dpto 2 dormitorios', 1),
(4, 'Dpto 3 dormitorios', 1),
(5, 'Casa con patio', 1),
(6, 'Casa con patio y pileta', 1),
(7, 'Casa sin patio', 1),
(8, 'Local comercial', 1),
(9, 'Oficina comercial', 1),
(10, 'Galpón - Apto depósito, taller, fábrica, etc.', 1),
(11, 'Galpón - Apto alimentos', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL,
  `nombre` varchar(30) NOT NULL,
  `apellido` varchar(30) NOT NULL,
  `email` varchar(50) NOT NULL,
  `pass` varchar(50) NOT NULL,
  `avatar` varchar(100) DEFAULT NULL,
  `isAdmin` tinyint(1) NOT NULL DEFAULT 0,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish2_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`idUsuario`, `nombre`, `apellido`, `email`, `pass`, `avatar`, `isAdmin`, `estado`) VALUES
(1, 'Admin', 'Istrador', 'admin@mail.com', '1yu065d/ZIryCaneVLbcLElwa4JAO+h9jjOVG7PsoKM=', '/Uploads\\avatar_1.jpg', 1, 1),
(2, 'Empleado', 'Del Mes', 'empleado@mail.com', '1yu065d/ZIryCaneVLbcLElwa4JAO+h9jjOVG7PsoKM=', NULL, 0, 1),
(3, 'Alexia', 'Verón', 'conavatar@gmail.com', '1yu065d/ZIryCaneVLbcLElwa4JAO+h9jjOVG7PsoKM=', '/Uploads\\avatar_3.webp', 0, 1),
(4, 'Carina', 'Piedrabuena', 'otroadmin@gmail.com', '1yu065d/ZIryCaneVLbcLElwa4JAO+h9jjOVG7PsoKM=', '/Uploads\\avatar_4.jpg', 1, 1),
(5, 'Mari', 'Perez', 'mp13@mail.com', '1yu065d/ZIryCaneVLbcLElwa4JAO+h9jjOVG7PsoKM=', NULL, 0, 1),
(6, 'Camila', 'Paez', 'camipaez@mail.com', '1yu065d/ZIryCaneVLbcLElwa4JAO+h9jjOVG7PsoKM=', NULL, 0, 0),
(7, 'Jorge', 'Cáceres', 'otroempleado@gmail.com', '1yu065d/ZIryCaneVLbcLElwa4JAO+h9jjOVG7PsoKM=', NULL, 0, 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`idContrato`),
  ADD KEY `idInmueble` (`idInmueble`),
  ADD KEY `idUsuarioAlta` (`idUsuarioAlta`),
  ADD KEY `idUsuarioBaja` (`idUsuarioBaja`),
  ADD KEY `idInquilino` (`idInquilino`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`idInmueble`),
  ADD KEY `idPropietario` (`idPropietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`idInquilino`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`idPago`),
  ADD KEY `idContrato` (`idContrato`),
  ADD KEY `idUsuarioAlta` (`idUsuarioAlta`),
  ADD KEY `idUsuarioBaja` (`idUsuarioBaja`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`idPropietario`);

--
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`idTipoInmueble`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`idUsuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `idContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `idInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `idInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `idPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `idPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `idTipoInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`idInmueble`) REFERENCES `inmueble` (`idInmueble`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`idUsuarioAlta`) REFERENCES `usuario` (`idUsuario`),
  ADD CONSTRAINT `contrato_ibfk_3` FOREIGN KEY (`idUsuarioBaja`) REFERENCES `usuario` (`idUsuario`),
  ADD CONSTRAINT `contrato_ibfk_4` FOREIGN KEY (`idInquilino`) REFERENCES `inquilino` (`idInquilino`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`idPropietario`) REFERENCES `propietario` (`idPropietario`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`idContrato`) REFERENCES `contrato` (`idContrato`),
  ADD CONSTRAINT `pago_ibfk_2` FOREIGN KEY (`idUsuarioAlta`) REFERENCES `usuario` (`idUsuario`),
  ADD CONSTRAINT `pago_ibfk_3` FOREIGN KEY (`idUsuarioBaja`) REFERENCES `usuario` (`idUsuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
