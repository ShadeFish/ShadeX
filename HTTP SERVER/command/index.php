<?php
include("../sql_connect.php");
$connection = GetConnection();

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $action = isset($_POST['action']) ? $_POST['action'] : '';
    $command_id = isset($_POST['command_id']) ? $_POST['command_id'] : '';
    $command_request = isset($_POST['command_request']) ? $_POST['command_request'] : '';
    $command_response = isset($_POST['command_response']) ? $_POST['command_response'] : '';
    $device_id = isset($_POST['device_id']) ? $_POST['device_id'] : '';

    /* SET RESPONSE COMMAND */
    if($action == "response_command")
    {
        $sql = "UPDATE device_commands SET command_response='{$command_response}' WHERE id='{$command_id}'";

        if($connection->query($sql))
        {
            echo "True";
        }
        exit();
    }

    /* GET ALL DEVICE COMMANDS */
    if($action == "get_device_all_commands")
    {

        $sql = "SELECT id, command_request, command_response FROM device_commands WHERE device_id='{$device_id}'";
        $result = $connection->query($sql);

        if($result->num_rows > 0)
        {
            while($row = $result->fetch_assoc())
            {
                if($row["command_response"] === NULL)
                {
                    echo $row["id"].":".$row["command_request"].",";
                }
                else
                {
                    echo $row["id"].":".$row["command_request"].":".$row["command_response"].",";
                }
            }
        }
        exit();
    }

    /* REQUEST COMMAND TO SINGLE DEVICE */
    if($action == "request_command_to_device")
    {
        $sql = "INSERT INTO device_commands (device_id,command_request) VALUES ('{$device_id}','{$command_request}')";
        if($connection->query($sql))
        {
            echo "True";
        }
        else
        {

            echo "request_command_to_device error";
        }
        exit();
    }

    /* REQUEST COMMAND TO ALL DEVICES */
    if($action == "request_command_to_all_devices")
    {
        $error = FALSE;
        $sql = "SELECT device_id FROM devices";
        $result = $connection->query($sql);

        if($result->num_rows > 0)
        {
            while($row = $result->fetch_assoc())
            {
                $device_id = $row["device_id"];
                $sql = "INSERT INTO device_commands (device_id,command_request) VALUES ('{$device_id}','{$command_request}')";
                if($connection->query($sql))
                {

                }
                else
                {
                    $error = TRUE;
                }
            }
            exit();
        }
        
        if($error === FALSE)
        {
            echo "True";
        }
        else
        {
            echo "request_command_to_all_devices error";
        }
        exit();
    }
}
?>