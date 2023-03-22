<?php
    define('SYNC_TYPE', 'Sync');

    $usData = array(
        "ID"=>0,
        "Login"=>$data["Login"],
        "Password"=>$data["Password"],
        "ErrorData"=>$errorData,
        "PlayerData"=>$playerData,
        "Place"=>1
    );

    if($data['Type'] == SYNC_TYPE)
    {
        //   =========================== USER ======================
        $user = $db->query("SELECT * FROM `users` WHERE `id` = '{$data["UserID"]}'")->fetch_assoc();
        $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$user['id']}'")->fetch_assoc();

        $teamMembers = $db->query("SELECT * FROM `teamMembers` WHERE `userID` = '{$user['id']}'");
    
        if($teamMembers->num_rows == 1)
        {
            $teamMember = $teamMembers->fetch_assoc();
            $team = $db->query("SELECT * FROM `teams` WHERE `id` = '{$teamMember["teamID"]}'")->fetch_assoc();

            $globalData['User']['PlayerData']['TeamData']['Name'] = $team["name"];
            $globalData['User']['PlayerData']['TeamData']['InviteCode'] = $team["inviteCode"];
            $globalData['User']['PlayerData']['TeamData']['Tickets'] = $team["tickets"];
            $globalData['User']['PlayerData']['TeamData']['Role'] = $teamMember['role'];
        }

        $globalData['User']['Login'] = $user['login'];
        $globalData['User']['PlayerData']['Tickets'] = $player["tickets"];
        $globalData['User']["Icon"] = $user["icon"];
        $globalData['User']['ID'] = $user["id"];
        
        //   =========================== TEAM LEADERS ======================

        $teams_rows = $db->query("SELECT * FROM `teams` ORDER BY `tickets` DESC");
        while($row = $teams_rows->fetch_assoc())
            $teams[] = $row;

        $k = 0;
        if(count($teams) >= 25)
            $k = 25;
        else
            $k = count($teams);

        for ($i=0; $i < $k; $i++) 
        { 
            $teamData['Name'] = $teams[$i]['name'];
            $teamData['Tickets'] = $teams[$i]['tickets'];
            $teamData['MembersCount'] = $teams[$i]['membersCount'];
    
            $teams_data[] = $teamData;
        }

        for ($i=0; $i < count($teams); $i++) 
        { 
            if($teams[$i]['name'] == $globalData['User']['PlayerData']['TeamData']['Name'])
                $globalData['TeamPlace'] = $i+1;
        }

        $globalData['TeamLeaders'] = json_encode($teams_data, JSON_UNESCAPED_UNICODE);

        //   =========================== PERSON LEADERS ======================

        $players_rows = $db->query("SELECT * FROM `players` ORDER BY `tickets` DESC");
        while($row = $players_rows->fetch_assoc())
            $players[] = $row;

        if(count($players) >= 50)
            $k = 50;
        else
            $k = count($players);

        for ($i=0; $i < $k; $i++) 
        { 
            $user = $db->query("SELECT * FROM `users` WHERE `id` = '{$players[$i]['userID']}'")->fetch_assoc();
            if($user['login'] != "EVENT_Bank_0")
            {
                $usData['Icon'] = $user['icon'];
                $usData['Login'] = $user['login'];
                $usData['PlayerData']['Tickets'] = $players[$i]['tickets'];
    
                $persons_data[] = $usData; 
            }
        }
        
        $globalData['PersonLeaders'] = json_encode($persons_data, JSON_UNESCAPED_UNICODE);

        for ($i=0; $i < count($players); $i++) 
        { 
            $user = $db->query("SELECT * FROM `users` WHERE `id` = '{$players[$i]['userID']}'")->fetch_assoc();
            if($user['login'] == $globalData['User']["Login"])
                $globalData['UserPlace'] = $i+1;
        }
        

        //   =========================== TEAM MEMBERS ======================

        $team = $db->query("SELECT * FROM `teams` WHERE `name` = '{$globalData['User']['PlayerData']['TeamData']['Name']}'")->fetch_assoc();
        $members_row = $db->query("SELECT * FROM `teamMembers` WHERE `teamID` = '{$team["id"]}'");
        
        while($row = $members_row->fetch_assoc())
            $members[] = $row;
            
        $members_data = [];
        for ($i=0; $i < count($members); $i++) 
        { 
            $user = $db->query("SELECT * FROM `users` WHERE `id` = '{$members[$i]['userID']}'")->fetch_assoc();
            $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$user['id']}'")->fetch_assoc();

            $usData['ID'] = $user['id'];
            $usData['Icon'] = $user['icon'];
            $usData['Login'] = $user['login'];
            $usData['PlayerData']['Tickets'] = $player['tickets'];
            $usData['PlayerData']['TeamData']['Role'] = $members[$i]['role'];

            $members_data[] = $usData;
        }

        $globalData['TeamMembers'] = json_encode($members_data, JSON_UNESCAPED_UNICODE);

        //   =========================== BONUSES ======================

        $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$globalData['User']['ID']}'")->fetch_assoc();
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
            $bonusData['AppearanceTime'] = $passedTime;
            $bonusData['Count'] = $bonuses[$i]['count'];
    
            $bonuses_data[] = $bonusData;
        }

        $globalData['Bonuses'] = json_encode($bonuses_data, JSON_UNESCAPED_UNICODE);
        echo json_encode($globalData, JSON_UNESCAPED_UNICODE);
    }
?>