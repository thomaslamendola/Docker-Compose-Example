FROM alpine:3.11.6
COPY ./bin/ /bin
EXPOSE 8500/tcp 8888/tcp
ENTRYPOINT ["/bin/hoverfly", "-listen-on-host=0.0.0.0"]