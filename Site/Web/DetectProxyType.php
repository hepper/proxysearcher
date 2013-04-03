<?php

/*foreach (getallheaders() as $name => $value) {
    echo "$name: $value<br />";
}*/

$headers = getallheaders();

if ($headers['Via'])
{
    if($headers['X-Forwarded-For'] != $headers['X-Remote-Addr'])
        echo 'Transparent';
    else
        echo 'Anonymous';
}
else
    echo 'HighAnonymous';

?>