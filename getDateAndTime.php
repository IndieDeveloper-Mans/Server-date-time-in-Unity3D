<?php
// Set your timezone
 date_default_timezone_set('Europe/Moscow');
 $currenttime = date("d-m-Y H:i:s");
 list($ddd,$ttt) = explode(' ',$currenttime);
 echo "$ddd/$ttt\n";
?>