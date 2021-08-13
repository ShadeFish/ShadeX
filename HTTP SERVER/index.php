<?php
include("sql_connect.php");
$connection = GetConnection();

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $action = isset($_POST['action']) ? $_POST['action'] : '';

    /* CONNECTION TEST */
    if($action == "test") { echo "True"; }
}

?>