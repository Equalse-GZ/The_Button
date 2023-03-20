<?php
    define('LEADERS_PERSON_TYPE', 'LeadersPerson');
    define('LEADERS_TEAMS_TYPE', 'LeadersTeam');
    define('LEADERS_TEAMSPLACE_TYPE', 'LeadersTeamPlace');
    define('LEADERS_PERSONPLACE_TYPE', 'LeadersPersonPlace');

    if($data['Type'] == LEADERS_PERSON_TYPE)
    {
        $players_rows = $db->query("SELECT * FROM `players` ORDER BY `tickets` DESC");
        while($row = $players_rows->fetch_assoc())
            $players[] = $row;

        $k = 0;
        if(count($players) >= 50)
            $k = 50;
        else
            $k = count($players);

        for ($i=0; $i < $k; $i++) 
        { 
            $userData['Login'] = $db->query("SELECT * FROM `users` WHERE `id` = '{$players[$i]['userID']}'")->fetch_assoc()['login'];
            $userData['PlayerData']['Tickets'] = $players[$i]['tickets'];
    
            $persons_data[] = $userData;
        }

        echo json_encode($persons_data, JSON_UNESCAPED_UNICODE);
    }



    if($data['Type'] == LEADERS_TEAMS_TYPE)
    {
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

        echo json_encode($teams_data, JSON_UNESCAPED_UNICODE);
    }




    if($data['Type'] == LEADERS_TEAMSPLACE_TYPE)
    {
        $teams_rows = $db->query("SELECT * FROM `teams` ORDER BY `tickets` DESC");
        while($row = $teams_rows->fetch_assoc())
            $teams[] = $row;
        
        for ($i=0; $i < count($teams); $i++) { 
            if($teams[$i]['name'] == $data['TeamName']){
                $teamData['Place'] = $i+1;
                echo json_encode($teamData, JSON_UNESCAPED_UNICODE);
                exit;
            }
        }
    }



    if($data['Type'] == LEADERS_PERSONPLACE_TYPE)
    {
        $players_rows = $db->query("SELECT * FROM `players` ORDER BY `tickets` DESC");
        while($row = $players_rows->fetch_assoc())
            $players[] = $row;

        for ($i=0; $i < count($players); $i++) { 
            $user = $db->query("SELECT * FROM `users` WHERE `id` = '{$players[$i]['userID']}'")->fetch_assoc();
            if($user['login'] == $data['UserName']){
                $userData['Place'] = $i+1;
                echo json_encode($userData, JSON_UNESCAPED_UNICODE);
                exit;
            }
        }
    }
?>