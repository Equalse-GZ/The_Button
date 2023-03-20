<?php
    define('BONUSES_GET_ALL_TYPE', 'BonusGetAll');
    define('BONUS_ADD_TYPE', 'BonusAdd');
    define('BONUS_DELETE_TYPE', 'BonusDelete');

    if($data['Type'] == BONUS_ADD_TYPE)
    {
        if($data['BonusType'] == "CONSTANT")
        {
            $bonuses = $db->query("SELECT * FROM `bonuses` WHERE `bonusID` = '{$data['BonusID']}'");
            if($bonuses->num_rows > 0)
            {
                $db->query("UPDATE `bonuses` SET `count` = '{$data['BonusCount']}' WHERE `bonusID` = '{$data["BonusID"]}'");
                exit;
            }
        }
        
       
        $date = date('Y-m-d H:i:s');
        
        $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$data['UserID']}'")->fetch_assoc();
        $db->query("INSERT INTO `bonuses` (`bonusID`, `playerID`, `type`, `appearanceTime`, `count`) VALUES ('{$data["BonusID"]}', '{$player["id"]}', '{$data["BonusType"]}', '{$date}', '{$data["BonusCount"]}')");
    }




    if($data['Type'] == BONUS_DELETE_TYPE)
    {
        $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$data['UserID']}'")->fetch_assoc();
        $bonus = $db->query("SELECT * FROM `bonuses` WHERE `bonusID` = '{$data['BonusID']}' AND `playerID` = '{$player['id']}'")->fetch_assoc();

        $db->query("DELETE FROM `bonuses` WHERE `bonusID` = '{$data['BonusID']}' AND `playerID` = '{$player['id']}' AND `appearanceTime` = '{$bonus['appearanceTime']}'");
    }





    if($data['Type'] == BONUSES_GET_ALL_TYPE)
    {
        $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$data['UserID']}'")->fetch_assoc();
        $bonuses_rows = $db->query("SELECT * FROM `bonuses` WHERE `playerID` = '{$player['id']}'");
        while($row = $bonuses_rows->fetch_assoc())
            $bonuses[] = $row;

        for ($i=0; $i < count($bonuses); $i++) 
        { 
            $bonusData['ID'] = $bonuses[$i]['bonusID'];
            $bonusData['Type'] = $bonuses[$i]['type'];
            
            $currentTime = explode("-", date('Y-m-d-H-i-s'));
            $datetime = explode(" ", $bonuses[$i]['appearanceTime']);
            $date = explode("-", $datetime[0]);
            $time = explode(":", $datetime[1]);
    
            $passedTime = (($currentTime[2] - $date[2]) * 24 * 60 * 60) + (($currentTime[3] - $time[0]) * 60 * 60) + (($currentTime[4] - $time[1]) * 60) + ($currentTime[5] - $time[2]);
            //$bonusData['AppearanceTime'] = $bonuses[$i]['appearanceTime'];
            $bonusData['AppearanceTime'] = $passedTime;
            $bonusData['Count'] = $bonuses[$i]['count'];
    
            $bonuses_data[] = $bonusData;
        }

        echo json_encode($bonuses_data, JSON_UNESCAPED_UNICODE);
    }
?>