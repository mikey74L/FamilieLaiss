$images = docker images -a -q
foreach ($image in $images) { docker image rm $image -f }