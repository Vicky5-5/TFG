-- phpMyAdmin SQL Dump
-- version 5.2.1deb1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost:3306
-- Tiempo de generación: 02-04-2025 a las 18:03:01
-- Versión del servidor: 10.11.6-MariaDB-0+deb12u1
-- Versión de PHP: 8.2.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `BD_Prueba`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Armas`
--

CREATE TABLE `Armas` (
  `idArma` int(10) UNSIGNED NOT NULL,
  `TIpo` enum('Blanca','Fuego') DEFAULT NULL,
  `Nombre` varchar(100) DEFAULT NULL,
  `Daño` int(11) DEFAULT NULL,
  `MunicionMAX` int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Enemigos`
--

CREATE TABLE `Enemigos` (
  `idEnemigo` int(10) UNSIGNED NOT NULL,
  `Tipo` varchar(45) DEFAULT NULL,
  `Vida` int(10) UNSIGNED DEFAULT NULL,
  `Daño` int(10) UNSIGNED DEFAULT NULL,
  `Defensa` int(11) DEFAULT NULL,
  `Nivel` int(10) UNSIGNED DEFAULT NULL,
  `Armas_idArma` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Items`
--

CREATE TABLE `Items` (
  `idItem` int(10) UNSIGNED NOT NULL,
  `Categoria` enum('Vida','Escudo') DEFAULT NULL,
  `Funcion` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Jefes`
--

CREATE TABLE `Jefes` (
  `idJefe` int(10) UNSIGNED NOT NULL,
  `Nombre` varchar(100) DEFAULT NULL,
  `Vida` int(10) UNSIGNED DEFAULT NULL,
  `Defensa` int(10) UNSIGNED DEFAULT NULL,
  `Daño` int(10) UNSIGNED DEFAULT NULL,
  `Velocidad` int(10) UNSIGNED DEFAULT NULL,
  `Nivel` int(10) UNSIGNED DEFAULT NULL,
  `Armas_idArma` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Personaje`
--

CREATE TABLE `Personaje` (
  `idPersonaje` int(10) UNSIGNED NOT NULL,
  `Vida` int(10) UNSIGNED DEFAULT NULL,
  `Escudo` int(10) UNSIGNED DEFAULT NULL,
  `Velocidad` int(10) UNSIGNED DEFAULT NULL,
  `Armas_idArma` int(10) UNSIGNED NOT NULL,
  `Items_idItem` int(10) UNSIGNED NOT NULL,
  `Usuarios_idJugador` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Usuarios`
--

CREATE TABLE `Usuarios` (
  `idJugador` int(10) UNSIGNED NOT NULL,
  `Usuario` varchar(45) NOT NULL,
  `Contraseña` varchar(45) NOT NULL,
  `FechaRegistro` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Volcado de datos para la tabla `Usuarios`
--

INSERT INTO `Usuarios` (`idJugador`, `Usuario`, `Contraseña`, `FechaRegistro`) VALUES
(1, 'Akira_04', '11112004', '2025-03-07'),
(2, 'Vicky', 'Vicky', '2025-03-07'),
(3, 'Raul', 'Raul', '2025-03-07');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `Armas`
--
ALTER TABLE `Armas`
  ADD PRIMARY KEY (`idArma`);

--
-- Indices de la tabla `Enemigos`
--
ALTER TABLE `Enemigos`
  ADD PRIMARY KEY (`idEnemigo`),
  ADD KEY `fk_Enemigos_Armas1_idx` (`Armas_idArma`);

--
-- Indices de la tabla `Items`
--
ALTER TABLE `Items`
  ADD PRIMARY KEY (`idItem`);

--
-- Indices de la tabla `Jefes`
--
ALTER TABLE `Jefes`
  ADD PRIMARY KEY (`idJefe`),
  ADD KEY `fk_Jefes_Armas1_idx` (`Armas_idArma`);

--
-- Indices de la tabla `Personaje`
--
ALTER TABLE `Personaje`
  ADD PRIMARY KEY (`idPersonaje`),
  ADD KEY `fk_Personaje_Armas_idx` (`Armas_idArma`),
  ADD KEY `fk_Personaje_Items1_idx` (`Items_idItem`),
  ADD KEY `fk_Personaje_Usuarios1_idx` (`Usuarios_idJugador`);

--
-- Indices de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  ADD PRIMARY KEY (`idJugador`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `Armas`
--
ALTER TABLE `Armas`
  MODIFY `idArma` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Enemigos`
--
ALTER TABLE `Enemigos`
  MODIFY `idEnemigo` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Items`
--
ALTER TABLE `Items`
  MODIFY `idItem` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Jefes`
--
ALTER TABLE `Jefes`
  MODIFY `idJefe` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Personaje`
--
ALTER TABLE `Personaje`
  MODIFY `idPersonaje` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  MODIFY `idJugador` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `Enemigos`
--
ALTER TABLE `Enemigos`
  ADD CONSTRAINT `fk_Enemigos_Armas1` FOREIGN KEY (`Armas_idArma`) REFERENCES `Armas` (`idArma`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `Jefes`
--
ALTER TABLE `Jefes`
  ADD CONSTRAINT `fk_Jefes_Armas1` FOREIGN KEY (`Armas_idArma`) REFERENCES `Armas` (`idArma`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `Personaje`
--
ALTER TABLE `Personaje`
  ADD CONSTRAINT `fk_Personaje_Armas` FOREIGN KEY (`Armas_idArma`) REFERENCES `Armas` (`idArma`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_Personaje_Items1` FOREIGN KEY (`Items_idItem`) REFERENCES `Items` (`idItem`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_Personaje_Usuarios1` FOREIGN KEY (`Usuarios_idJugador`) REFERENCES `Usuarios` (`idJugador`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
