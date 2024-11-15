<?php
include "testdb.php";

$dt = $_POST;

$cardStats = [
    "id" => 1,
    "Name"=>"Name",
    "Desc" => "Null",
];

$error = [
    "errorText" => "empty",
    "isError" => false
];

$cardData = [
    "cardStats"=>$cardStats,
    "error"=>$error
];

ob_end_clean();
if(isset($dt['type'])) {
    if ($dt['type'] == "Download") {
        if (isset($dt['id'])) {
            $cards = $db->query("SELECT * FROM `testcardtable` WHERE `id` = '{$dt['id']}'");

            if ($cards->rowCount() == 1) {
                $card = $cards->fetch(PDO::FETCH_ASSOC);        
                $cardData["cardStats"]["id"] = $card["id"];
                $cardData["cardStats"]["Name"] = $card["Name"];
                $cardData["cardStats"]["Desc"] = $card["Description"]; 
            }
            else {
                SetError("Somehow there are two copies of that card");
            }
        }
        else {
            SetError("Id not set");
        }

        $encoded_data = json_encode($cardData, JSON_UNESCAPED_UNICODE);
        echo $encoded_data;

    } 

    else if ($dt['type'] == "Upload") {
        if (isset($dt['name']) && isset($dt['desc'])) {
            $cards = $db->query("SELECT * FROM `testcardtable` WHERE `Name` = '{$dt['name']}'");

            if ($cards->rowCount() == 0) {
                $db->query("INSERT INTO `testcardtable`(`Name`, `Description`) VALUES('{$dt['name']}','{$dt['desc']}')");
                $cards = $db->query("SELECT * FROM `testcardtable` WHERE `Name` = '{$dt['name']}' and `Description` = '{$dt['desc']}'");
            }
            else {
                SetError("Card already exists");
            }
        }
        else {
            SetError("Parameters not set");
        }

        $encoded_data = json_encode($cardData, JSON_UNESCAPED_UNICODE);
        echo $encoded_data;
        
    }

    else if ($dt['type'] == "DownloadAll") {
        $rows = $db->query("SELECT * FROM `testcardtable`");
        echo $rows->rowCount(); 
    }

    else {
        SetError("Type is not correct");
    }

    function SetError($text) {
        global $cardData;
        $cardData["cardStats"] = null;
        $cardData["error"]["isError"] = true;
        $cardData["error"]["errorText"] = "Error: ".$text;
    }
}
?>