# Created By Michael Laiss

FROM tomcat:8.5.15-jre8-alpine
MAINTAINER Michael Laiss <mikey74@hotmail.de>

RUN rm -fr /usr/local/tomcat/webapps/ROOT &&\
mkdir /usr/local/tomcat/webapps/ROOT && \
cd /usr/local/tomcat/webapps/ROOT/ && \
wget -q http://repo1.maven.org/maven2/com/netflix/eureka/eureka-server/1.9.9/eureka-server-1.9.9.war &&\
unzip eureka-server-1.9.9.war &&\
rm eureka-server-1.9.9.war

COPY ["Eureka/config.properties", "/usr/local/tomcat/webapps/ROOT/config.properties"]
COPY ["Eureka/eureka-client-test.properties", "/usr/local/tomcat/webapps/ROOT/WEB-INF/classes/eureka-client-test.properties"]
COPY ["Eureka/eureka-server-test.properties", "/usr/local/tomcat/webapps/ROOT/WEB-INF/classes/eureka-server-test.properties"]
  
EXPOSE 8080

ENTRYPOINT ["/usr/local/tomcat/bin/catalina.sh"]

CMD ["run"]