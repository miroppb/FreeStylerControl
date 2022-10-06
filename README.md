# FSControl

Small application to control FreeStyler lights. Created to be used in our church, as sometimes lights are turned on, but not turned off.

If someone needs to see how FreeStyler can be controlled via TCP/IP, more than welcome to see for yourself.

## Provides

* Ability to toggle all light groups
* Power all On
* Power all Off
* Run a set of commands to turn on specific lights and specific colors
* Api for control from a browser
    * Same set of buttons as main application
    * Authenticating users
    * Registering users

## Database
```
CREATE DATABASE IF NOT EXISTS `fscontrol` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `fscontrol`;

-- Dumping structure for table fscontrol.schedule
CREATE TABLE IF NOT EXISTS `schedule` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `day` int(1) NOT NULL DEFAULT 0,
  `time` text NOT NULL,
  `action` int(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Dumping structure for table fscontrol.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(5) NOT NULL AUTO_INCREMENT,
  `username` text NOT NULL,
  `hash` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
```