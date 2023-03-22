<?php
    define('DONATION_TYPE', 'EventDonate');

    if($data['Type'] == DONATION_TYPE)
    {
        $user = $db->query("SELECT * FROM `users` WHERE `login` = 'EVENT_Bank_0'")->fetch_assoc();
        $bank = $db->query("SELECT * FROM `players` WHERE `userID` = '{$user['id']}'")->fetch_assoc();
        $newTicketsValue = $bank['tickets'] + 1;
        $db->query("UPDATE `players` SET `tickets` = '{$newTicketsValue}}' WHERE `userID` = '{$user['id']}'");
        echo json_encode($userData, JSON_UNESCAPED_UNICODE);
    }
?>