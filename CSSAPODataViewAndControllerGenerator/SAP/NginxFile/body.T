server {
        listen #portNumber#;
        listen [::]:#portNumber#;
        
        root #indexFilePath#;
        server_name #subdomain#.#domain#;
        index index.html index.htm index.nginx-debian.html;


        location / {
                try_files $uri $uri/ =404;
        }

}