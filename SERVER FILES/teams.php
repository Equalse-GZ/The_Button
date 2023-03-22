<?php
    define('TEAM_CREATE_TYPE', 'TeamCreate');
    define('TEAM_LEAVE_TYPE', 'TeamLeave');
    define('TEAM_CONNECT_TYPE', 'TeamConnect');
    define('TEAM_ADDTICKETS_TYPE', 'TeamAddTickets');
    define('TEAM_GETTICKETS_TYPE', 'TeamGetTickets');
    define('TEAM_MEMBERS_TYPE', 'TeamMembers');

    define('TEAM_NAME', 'TeamName');
    
    if($data['Type'] == TEAM_CREATE_TYPE)
    {
        $teams = $db->query("SELECT * FROM `teams` WHERE `name` = '{$data[TEAM_NAME]}'");
        $team = $teams->fetch_assoc();

        $teamMembers = $db->query("SELECT * FROM `teamMembers` WHERE `userID` = '{$data["UserID"]}'");
        if($teamMembers->num_rows > 0)
        {
            $teamData['ErrorData']['IsEmpty'] = false;
            $teamData['ErrorData']['Message'] = "У Вас уже есть созданная команда!";
            echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
            exit;
        }

        if($teams->num_rows == 0)
        {
            $inviteCodeChars = "ZAQXSWCDEVFRBGTNHYMJUKILOP1234567890";
            $inviteCode = null;
            for ($i=0; $i < 3; $i++) 
            { 
                for ($j=0; $j < 3; $j++) 
                    $inviteCode.=$inviteCodeChars[rand(0, strlen($inviteCodeChars) - 1)];
            
                if ($i < 2)
                    $inviteCode.="-";
            }
            $db->query("INSERT INTO `teams` (`name`, `membersCount`, `inviteCode`, `tickets`) VALUES ('{$data["TeamName"]}', 1, '{$inviteCode}', 0)");
            $teams = $db->query("SELECT * FROM `teams` WHERE `name` = '{$data[TEAM_NAME]}'");
            $team = $teams->fetch_assoc();

            $db->query("INSERT INTO `teamMembers` (`teamID`, `userID`, `role`) VALUES ('{$team["id"]}', '{$data["UserID"]}', '{$data["Role"]}')");
            $teamData["Name"] = $data["TeamName"];
            $teamData["InviteCode"] = $inviteCode;
            $teamData["Tickets"] = 0;
            $teamData["Role"] = "Admin";
            echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
        }
        else
        {
            $teamData['ErrorData']['IsEmpty'] = false;
            $teamData['ErrorData']['Message'] = "Команда с таким именем уже существует!";
            echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
            exit;
        }
    }






    if($data['Type'] == TEAM_LEAVE_TYPE)
    {
        $teams = $db->query("SELECT * FROM `teams` WHERE `name` = '{$data[TEAM_NAME]}'");
        $team = $teams->fetch_assoc();

        $teamMembers = $db->query("SELECT * FROM `teamMembers` WHERE `userID` = '{$data["UserID"]}'");
        $teamMember = $teamMembers->fetch_assoc();
        if($teamMember['role'] == "Admin" && $team['membersCount'] > 1)
        {
            $members_row = $db->query("SELECT * FROM `teamMembers` WHERE `teamID` = '{$team["id"]}'");
            while($row = $members_row->fetch_assoc())
                $members[] = $row;

            $db->query("UPDATE `teamMembers` SET `role` = 'Admin' WHERE `userID` = '{$members[1]['userID']}'");
        }


        $db->query("DELETE FROM `teamMembers` WHERE `userID` = '{$data["UserID"]}'");
        $newMembersCount = $db->query("SELECT * FROM `teamMembers` WHERE `teamID` = '{$team["id"]}'")->num_rows;
        if($team['membersCount'] == 1)
        {
            $teamMembers = $db->query("SELECT * FROM `teamMembers` WHERE `userID` = '{$data["UserID"]}'");
            $teamData['Tickets'] = $team['tickets'];
            $db->query("DELETE FROM `teams` WHERE `name` = '{$team["name"]}'");
        }
        else
            $db->query("UPDATE `teams` SET `membersCount` = '{$newMembersCount}' WHERE `name` = '{$team["name"]}'");
            
        echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
    }






    if($data['Type'] == TEAM_CONNECT_TYPE)
    {
        $teams = $db->query("SELECT * FROM `teams` WHERE `inviteCode` = '{$data["InviteCode"]}'");
        if($teams->num_rows == 0)
        {
            $teamData['ErrorData']['IsEmpty'] = false;
            $teamData['ErrorData']['Message'] = "Команды не существует!";
            echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
            exit;
        }

        $team = $teams->fetch_assoc();
        $db->query("INSERT INTO `teamMembers` (`teamID`, `userID`, `role`) VALUES ('{$team["id"]}', '{$data["UserID"]}', '{$data["Role"]}')");

        $newMembersCount = $db->query("SELECT * FROM `teamMembers` WHERE `teamID` = '{$team["id"]}'")->num_rows;
        $db->query("UPDATE `teams` SET `membersCount` = '{$newMembersCount}' WHERE `inviteCode` = '{$data["InviteCode"]}'");

        $teamData["Name"] = $team["name"];
        $teamData["InviteCode"] = $data["InviteCode"];
        $teamData["Tickets"] = $team["tickets"];
        echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
    }




    if($data['Type'] == TEAM_GETTICKETS_TYPE)
    {
        $teams = $db->query("SELECT * FROM `teams` WHERE `name` = '{$data[TEAM_NAME]}'");
        $team = $teams->fetch_assoc();

        if($team['tickets'] - $data['Tickets'] < 0)
        {
            $teamData['ErrorData']['IsEmpty'] = false;
            $teamData['ErrorData']['Message'] = "В казне недостаточно средств!";
            echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
            exit;
        }

        $newTicketsValue = $team['tickets'] - $data['Tickets'];
        $db->query("UPDATE `teams` SET `tickets` = '{$newTicketsValue}' WHERE `name` = '{$data[TEAM_NAME]}'");

        $teamData["Name"] = $team["name"];
        $teamData["InviteCode"] = $team["inviteCode"];
        $teamData["Tickets"] = $newTicketsValue;
        echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
    }
    
    
    
    
    if($data['Type'] == TEAM_MEMBERS_TYPE)
    {
        $teams = $db->query("SELECT * FROM `teams` WHERE `name` = '{$data[TEAM_NAME]}'");
        $team = $teams->fetch_assoc();
        $members_row = $db->query("SELECT * FROM `teamMembers` WHERE `teamID` = '{$team["id"]}'");
        
        while($row = $members_row->fetch_assoc())
            $members[] = $row;
            
        $members_data = [];
        for ($i=0; $i < count($members); $i++) 
        { 
            $user = $db->query("SELECT * FROM `users` WHERE `id` = '{$members[$i]['userID']}'")->fetch_assoc();
            $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$user['id']}'")->fetch_assoc();

            $userData['ID'] = $user['id'];
            $userData['Icon'] = $user['icon'];
            $userData['Login'] = $user['login'];
            $userData['PlayerData']['Tickets'] = $player['tickets'];
            $userData['PlayerData']['TeamData']['Role'] = $members[$i]['role'];

            $members_data[] = $userData;
        }

        echo json_encode($members_data, JSON_UNESCAPED_UNICODE);
    }




    if($data['Type'] == TEAM_ADDTICKETS_TYPE)
    {
        $teams = $db->query("SELECT * FROM `teams` WHERE `name` = '{$data[TEAM_NAME]}'");
        $team = $teams->fetch_assoc();

        $newTicketsValue = $team['tickets'] + $data['Tickets'];
        $db->query("UPDATE `teams` SET `tickets` = '{$newTicketsValue}' WHERE `name` = '{$data[TEAM_NAME]}'");
        
        $teamData["Name"] = $team["name"];
        $teamData["InviteCode"] = $team["inviteCode"];
        $teamData["Tickets"] = $newTicketsValue;
        echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
    }
?>