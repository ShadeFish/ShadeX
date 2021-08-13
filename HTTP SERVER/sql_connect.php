<?php

function GetConnection()
{
    $server_name = "localhost";
    $user_name = "root";
    $password = "";
    $database = "myDB";

    $connection = mysqli_connect($server_name,$user_name,$password);
    mysqli_select_db($connection, $database);
    return $connection;
}


?>