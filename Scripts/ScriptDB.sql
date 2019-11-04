CREATE TABLE `Author` (
    `AuthorID` int(10) NOT NULL AUTO_INCREMENT, 
          `Name` VarChar(255) NOT NULL ,
              PRIMARY KEY (`AuthorID`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
 
CREATE TABLE `Post` (
    `PostID` int(10) NOT NULL AUTO_INCREMENT,  
    `Title` varchar(255) NOT NULL, 
    `Text` longtext NOT NULL, 
    `Created` datetime NOT NULL, 
    `AuthorID` int(10)  NOT NULL  ,
              PRIMARY KEY (`PostID`)
)    ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
