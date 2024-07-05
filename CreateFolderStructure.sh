# ******************************
# Create directories for content
# ******************************
DIR2="/var/lib/familielaiss/Content"
if [ ! -d "$DIR2" ]; then
  echo "${DIR2} not found. Create folder." 
  mkdir "$DIR2"
  chmod 777 -R "$DIR2"
fi

DIR3="/var/lib/familielaiss/Content/ConvertVideo"
if [ ! -d "$DIR3" ]; then
  echo "${DIR3} not found. Create folder."
  mkdir "$DIR3"
  chmod 777 -R "$DIR3"
fi

DIR4="/var/lib/familielaiss/Content/Picture"
if [ ! -d "$DIR4" ]; then
  echo "${DIR4} not found. Create folder."
  mkdir "$DIR4"
  chmod 777 -R "$DIR4"
fi

DIR5="/var/lib/familielaiss/Content/Portrait"
if [ ! -d "$DIR5" ]; then
  echo "${DIR5} not found. Create folder."
  mkdir "$DIR5"
  chmod 777 -R "$DIR5"
fi

DIR6="/var/lib/familielaiss/Content/StandardPicture"
if [ ! -d "$DIR6" ]; then
  echo "${DIR6} not found. Create folder."
  mkdir "$DIR6"
  chmod 777 -R "$DIR6"
fi

DIR7="/var/lib/familielaiss/Content/Video"
if [ ! -d "$DIR7" ]; then
  echo "${DIR7} not found. Create folder."
  mkdir "$DIR7"
  chmod 777 -R "$DIR7"
fi


# *************************
# Create directories for db
# *************************
DIR8="/var/lib/familielaiss/db"
if [ ! -d "$DIR8" ]; then
  echo "${DIR8} not found. Create folder."
  mkdir "$DIR8"
  chmod 777 -R "$DIR8"
fi

DIR9="/var/lib/familielaiss/db/blog"
if [ ! -d "$DIR9" ]; then
  echo "${DIR9} not found. Create folder."
  mkdir "$DIR9"
  chmod 777 -R "$DIR9"
fi

DIR10="/var/lib/familielaiss/db/catalog"
if [ ! -d "$DIR10" ]; then
  echo "${DIR10} not found. Create folder."
  mkdir "$DIR10"
  chmod 777 -R "$DIR10"
fi

DIR11="/var/lib/familielaiss/db/identity"
if [ ! -d "$DIR11" ]; then
  echo "${DIR11} not found. Create folder."
  mkdir "$DIR11"
  chmod 777 -R "$DIR11"
fi

DIR12="/var/lib/familielaiss/db/mail"
if [ ! -d "$DIR12" ]; then
  echo "${DIR12} not found. Create folder."
  mkdir "$DIR12"
  chmod 777 -R "$DIR12"
fi

DIR13="/var/lib/familielaiss/db/message"
if [ ! -d "$DIR13" ]; then
  echo "${DIR13} not found. Create folder."
  mkdir "$DIR13"
  chmod 777 -R "$DIR13"
fi

DIR14="/var/lib/familielaiss/db/pictureconvert"
if [ ! -d "$DIR14" ]; then
  echo "${DIR14} not found. Create folder."
  mkdir "$DIR14"
  chmod 777 -R "$DIR14"
fi

DIR15="/var/lib/familielaiss/db/scheduler"
if [ ! -d "$DIR15" ]; then
  echo "${DIR15} not found. Create folder."
  mkdir "$DIR15"
  chmod 777 -R "$DIR15"
fi

DIR16="/var/lib/familielaiss/db/settings"
if [ ! -d "$DIR16" ]; then
  echo "${DIR16} not found. Create folder."
  mkdir "$DIR16"
  chmod 777 -R "$DIR16"
fi

DIR17="/var/lib/familielaiss/db/upload"
if [ ! -d "$DIR17" ]; then
  echo "${DIR17} not found. Create folder."
  mkdir "$DIR17"
  chmod 777 -R "$DIR17"
fi

DIR18="/var/lib/familielaiss/db/userinteraction"
if [ ! -d "$DIR18" ]; then
  echo "${DIR18} not found. Create folder."
  mkdir "$DIR18"
  chmod 777 -R "$DIR18"
fi

DIR19="/var/lib/familielaiss/db/videoconvert"
if [ ! -d "$DIR19" ]; then
  echo "${DIR19} not found. Create folder."
  mkdir "$DIR19"
  chmod 777 -R "$DIR19"
fi


# ****************************
# Create directory for Secrets
# ****************************
DIR21="/var/lib/familielaiss/Secrets"
if [ ! -d "$DIR21" ]; then
  echo "${DIR21} not found. Create folder."
  mkdir "$DIR21"
  chmod 777 -R "$DIR21"
fi


# ****************************
# Create directory for pgadmin
# ****************************
DIR22="/var/lib/familielaiss/pgadmin"
if [ ! -d "$DIR22" ]; then
  echo "${DIR22} not found. Create folder."
  mkdir "$DIR22"
  chmod 777 -R "$DIR22"
fi


# *****************************
# Create directory for rabbitmq
# *****************************
DIR23="/var/lib/familielaiss/rabbitmq"
if [ ! -d "$DIR23" ]; then
  echo "${DIR23} not found. Create folder."
  mkdir "$DIR23"
  chmod 777 -R "$DIR23"
fi


# ********************************
# Create folder for docker-compose
# ********************************
DIR24="/var/lib/familielaiss/compose"
if [ ! -d "$DIR24" ]; then
  echo "${DIR24} not found. Create folder."
  mkdir "$DIR24"
  chmod 777 -R "$DIR24"
fi


# ********************************
# Create folder for logging
# ********************************
DIR25="/var/lib/familielaiss/logging"
if [ ! -d "$DIR25" ]; then
  echo "${DIR25} not found. Create folder."
  mkdir "$DIR25"
  chmod 777 -R "$DIR25"
fi
