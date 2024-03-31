<?php
include "db.php";

$dt = $_GET;

$CardData = array(
    "id" => 0,
    "Name"=>"Name",
    "Desc" => "Null",
    "Image" => "Null",
    "ManaCost" => 0,
    "Health"=> 0,
    "Damage" => 0,
    "Race" => "Null",
    "CardType" => "Null",
);

if (isset($dt['type']) == "loading") {
    if (isset($dt['name'])) {
        // $users = $db->query("SELECT * FROM `users` WHERE `login` = '{$dt['login']}'");
    }
} 

json_encode($CardData, JSON_UNESCAPED_UNICODE);
echo "Hello";
?>