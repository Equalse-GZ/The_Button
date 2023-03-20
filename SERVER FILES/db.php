<?php
    define('DB_HOST', 'localhost');
    define('DB_USER', 'equals36_db');
    define('DB_PASSWORD', '@8aWSWN$~*');
    define('DB_NAME', 'equals36_db');

    $db = new mysqli(DB_HOST, DB_USER, DB_PASSWORD, DB_NAME);

    if($db->connect_errno) exit('Ошибка подключения к БД');
    $db->set_charset('utf8');
?>