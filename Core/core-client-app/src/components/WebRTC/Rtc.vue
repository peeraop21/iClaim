<template>
    <div>
        <div id="camera">
            <video id="stream"></video>

            <div id="snapCam" @click="onClickBtnCapture">
                <span></span>
                <span></span>
                <span></span>
            </div>
            <div id="change-camera" @click="changeCamMode">
                <span></span>
                <span><b-icon icon="arrow-repeat" font-scale="3" style="color: white; opacity:1"></b-icon></span>

            </div>
        </div>
        <canvas id="canvas"></canvas>
        <div id="snapshot">
            <img src="" id="img1">
            <div id="accept-img" @click="acceptImage">
                <span></span>
                <span><b-icon icon="check2" font-scale="3" style="color: white; opacity:1"></b-icon></span>
            </div>
            <div id="cansel-img">
                <span></span>
                <span><b-icon icon="x" font-scale="3" style="color: white; opacity:1"></b-icon></span>
            </div>
        </div>

        <div class="button-group">
            <button hidden id="btnStream" type="button" class="button" @click="onClickBtnStream">Take a photo</button>
            <label hidden id="picture" for="file" class="label custom-file-upload">Browse an image</label>
        </div>
    </div>
</template>

<script>

    import liff from '@line/liff'
    export default {
        name: "Rtc",
        components: {
        },
        data() {
            return {
                cameraStream: null,
                files: null,
                constraints: {
                    audio: false,
                    video: {
                        width: 1920,
                        height: 1080,
                        facingMode: "environment"
                    },
                },
            }
        },
        props: ['active'],
        watch: {
            active: function (newVal, oldVal) {
                if (newVal == false && oldVal == true) {
                    this.stopStreaming()
                }
            }
        },
        methods: {
            changeCamMode() {
                this.stopStreaming();
                if (this.constraints.video.facingMode == 'environment') {
                    this.constraints.video.facingMode = 'user'
                    this.startStreaming();
                } else {
                    this.constraints.video.facingMode = 'environment'
                    this.startStreaming();
                }
            },
            startStreaming() {
                this.stopStreaming()
                const stream = document.querySelector('#stream')
                const mediaSupport = 'mediaDevices' in navigator

                if (mediaSupport && (null == this.cameraStream || this.cameraStream == '')) {
                    navigator.mediaDevices.getUserMedia(this.constraints).then((mediaStream) => {
                        this.cameraStream = mediaStream
                        stream.srcObject = mediaStream
                        stream.play()
                    }).catch((err) => {
                        alert(err)
                    })
                } else {
                    alert('Your browser does not support media devices.')
                    return
                }
            },
            captureSnapshot() {
                const previewImage = document.querySelector('#snapshot #img1')
                const stream = document.querySelector('#stream')
                const canvas = document.querySelector('#canvas')
                if (null != this.cameraStream) {
                    canvas.width = stream.videoWidth;
                    canvas.height = stream.videoHeight;
                    var ctx = canvas.getContext('2d')
                    ctx.drawImage(stream, 0, 0)
                    previewImage.src = canvas.toDataURL("image/png")
                    this.files = canvas.toDataURL("image/png")
                }
            },
            stopStreaming() {
                const stream = document.querySelector('#stream')
                if (null != this.cameraStream) {
                    const track = this.cameraStream.getTracks()[0]
                    track.stop()
                    stream.load()
                    this.cameraStream = null
                }
            },
            onClickBtnStream() {
                const camera = document.querySelector('#camera')
                const snapshot = document.querySelector('#snapshot')
                this.startStreaming()
                camera.style.display = "block"
                snapshot.style.display = "none"
            },
            onClickBtnCapture() {
                const camera = document.querySelector('#camera')
                const snapshot = document.querySelector('#snapshot')
                this.captureSnapshot()
                camera.style.display = "none"
                snapshot.style.display = "block"
                this.stopStreaming()
            },
            acceptImage() {
                this.stopStreaming();
                this.$emit('base64', this.files)
            }
        },
        created() {
            liff.init({
                liffId: this.$store.state.liffId,
            }).then(() => {
                this.onClickBtnStream()
            })
        },
    }
</script>

<style scoped>
    body {
        padding: 0;
        margin: 0;
    }

    #container {
        padding: 16px;
    }

    #canvas, #camera, input[type=file] {
        display: none
    }

    .label {
        display: inline-block;
        margin: 4px
    }

    .button {
        width: 100%;
        background-color: #06c755;
        color: white;
        padding: 14px 20px;
        margin: 8px 0;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-size: 16px;
        font-weight: bold;
        height: 58px;
    }

    .button-group button {
        width: 49.4%
    }

    .disable {
        background-color: #efefef;
        color: #777;
    }

    #stream, #snapshot img, #canvas {
        /*inline-size: -webkit-fill-available;*/
        width: 100%;
    }

    .custom-file-upload {
        width: 49.4%;
        border-radius: 4px;
        background-color: #06c755;
        padding: 20px 0;
        cursor: pointer;
        margin: 8px 0;
        color: white;
        font-weight: bold;
        font-size: 16px;
        text-align: center
    }

    #camera {
        position: relative;
    }

        #camera div {
            cursor: pointer;
        }

        #camera #snapCam span {
            position: absolute;
            border-radius: 50%;
            bottom: 20px;
            left: 0;
            right: 0;
            margin-left: auto;
            margin-right: auto;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            #camera #snapCam span:first-of-type {
                border-radius: 50%;
                width: 80px;
                height: 80px;
            }

            #camera #snapCam span:nth-of-type(2) {
                background: gray;
                opacity: 0.5;
                bottom: 24px;
                width: 72px;
                height: 72px;
            }

            #camera #snapCam span:last-of-type {
                background: white;
                bottom: 28px;
                width: 64px;
                height: 64px;
            }

        #camera #change-camera {
            cursor: pointer;
        }

            #camera #change-camera span {
                position: absolute;
                border-radius: 50%;
                bottom: 20px;
                left: 65%;
                right: 0;
                top: 80%;
                margin-left: auto;
                margin-right: auto;
                display: flex;
                align-items: center;
                justify-content: center;
            }

                #camera #change-camera span:first-of-type {
                    background: gray;
                    opacity: 0.5;
                    bottom: 28px;
                    width: 50px;
                    height: 50px;
                }

                #camera #change-camera span:last-of-type {
                    bottom: 28px;
                    width: 50px;
                    height: 50px;
                }


    #snapshot {
        position: relative;
    }

        #snapshot div {
            cursor: pointer;
        }

        #snapshot #accept-img span {
            position: absolute;
            border-radius: 50%;
            left: 65%;
            right: 0;
            top: 80%;
            margin-left: auto;
            margin-right: auto;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            #snapshot #accept-img span:first-of-type {
                background: gray;
                opacity: 0.5;
                bottom: 28px;
                width: 50px;
                height: 50px;
            }

            #snapshot #accept-img span:last-of-type {
                bottom: 28px;
                width: 50px;
                height: 50px;
            }



        #snapshot #cansel-img span {
            position: absolute;
            border-radius: 50%;
            left: -65%;
            right: 0;
            top: 80%;
            margin-left: auto;
            margin-right: auto;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            #snapshot #cansel-img span:first-of-type {
                background: gray;
                opacity: 0.5;
                bottom: 28px;
                width: 50px;
                height: 50px;
            }

            #snapshot #cansel-img span:last-of-type {
                bottom: 28px;
                width: 50px;
                height: 50px;
            }

    img#img1 {
        width: 100%;
    }

    @media only screen and (min-width: 960px) {
        #container {
            width: 36%;
            margin: 0 auto
        }

        #stream, #snapshot img, #canvas {
            height: 100%;
        }
    }
</style>