<?php
    $data = $_POST;

    $errorData = array(
        "IsEmpty"=>true,
        "Message"=>"Empty"
    );
    
    $teamData = array(
        "Name"=>"",
        "MembersCount"=>1,
        "InviteCode"=>"code",
        "Tickets"=>0,
        "ErrorData"=>$errorData,
        "Role"=>"Member",
        "Place"=>1
    );
    
    $bonusData = array(
        "ID"=>0,
        "Type"=>"",
        "AppearanceTime"=>"",
        "Count"=>1
    );
    
    $playerData = array(
        "Tickets"=>0,
        "TeamData"=>$teamData
    );
    
    $userData = array(
        "ID"=>0,
        "Login"=>$data["Login"],
        "Password"=>$data["Password"],
        "ErrorData"=>$errorData,
        "PlayerData"=>$playerData,
        "Place"=>1
    );
?>