<?php
    define('LOGIN', 'Login');
    define('PASSWORD', 'Password');
    define('REG_TYPE', 'Registering');
    define('LOG_TYPE', 'Logging');
    define('ICON', 'Icon');
    define('SAVE_TYPE', 'Save');

    if($data['Type'] == REG_TYPE)
    {
        $users = $db->query("SELECT * FROM `users` WHERE `login` = '{$data[LOGIN]}'");
        if($users->num_rows == 0)
        {
            $hash_password = password_hash($data[PASSWORD], PASSWORD_DEFAULT);
            $db->query("INSERT INTO `users` (`login`, `password`, `icon`) VALUES ('{$data[LOGIN]}', '{$hash_password}', '{$data[ICON]}')");
            $db->query("INSERT INTO `players`(`userID`, `tickets`) VALUES ('{$db->insert_id}', 0)");
            echo json_encode($userData, JSON_UNESCAPED_UNICODE);
        }
        else
        {
            $userData['ErrorData']['IsEmpty'] = false;
            $userData['ErrorData']['Message'] = "Такой пользователь уже существует!";
            echo json_encode($userData, JSON_UNESCAPED_UNICODE);;
        }
    }

    if($data['Type'] == LOG_TYPE)
    {
        $users = $db->query("SELECT * FROM `users` WHERE `login` = '{$data[LOGIN]}'");
        $user = $users->fetch_assoc();
            
        
        if($users->num_rows == 0 || !password_verify($data[PASSWORD], $user['password'])){
            $userData['ErrorData']['IsEmpty'] = false;
            $userData['ErrorData']['Message'] = "Неверный логин или пароль!";
            echo json_encode($userData, JSON_UNESCAPED_UNICODE);
            exit;
        }
                
        $player = $db->query("SELECT * FROM `players` WHERE `userID` = '{$user['id']}'")->fetch_assoc();
        $teamMembers = $db->query("SELECT * FROM `teamMembers` WHERE `userID` = '{$user['id']}'");
    
        if($teamMembers->num_rows == 1)
        {
            $teamMember = $teamMembers->fetch_assoc();
            $teams = $db->query("SELECT * FROM `teams` WHERE `id` = '{$teamMember["teamID"]}'");
            $team = $teams->fetch_assoc();

            $userData['PlayerData']['TeamData']['Name'] = $team["name"];
            $userData['PlayerData']['TeamData']['InviteCode'] = $team["inviteCode"];
            $userData['PlayerData']['TeamData']['Tickets'] = $team["tickets"];
            $userData['PlayerData']['TeamData']['Role'] = $teamMember['role'];
        }

        $userData['PlayerData']['Tickets'] = $player["tickets"];
        
        $userData["Icon"] = $user["icon"];
        $userData['ID'] = $user["id"];
        echo json_encode($userData, JSON_UNESCAPED_UNICODE);
    }
    
    if($data['Type'] == SAVE_TYPE)
    {
        $db->query("UPDATE `players` SET `tickets` = '{$data["Tickets"]}' WHERE `userID` = '{$data["ID"]}'");
    }
?>