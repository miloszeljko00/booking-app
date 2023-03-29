FROM quay.io/keycloak/keycloak:legacy

# Copy the realm configuration file to the image
COPY realm-export.json /tmp/realm-export.json

WORKDIR /opt/jboss/keycloak/themes
RUN rm -r -f ./base
COPY /base ./base
COPY /blusky ./blusky
COPY /vodafone ./vodafone
COPY /digitalcity ./digitalcity

# Use the Keycloak API to import the realm configuration at container startup
RUN echo '/opt/jboss/keycloak/bin/kcadm.sh config credentials --server http://localhost:8080/auth --realm master --user admin --password admin' >> /opt/jboss/startup-scripts/startup.sh && \
    echo '/opt/jboss/keycloak/bin/kcadm.sh create realms -f /tmp/realm-export.json' >> /opt/jboss/startup-scripts/startup.sh && \
    chmod +x /opt/jboss/startup-scripts/startup.sh