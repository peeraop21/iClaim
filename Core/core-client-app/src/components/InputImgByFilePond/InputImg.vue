<template>
    <div>
        <file-pond credits="null"
                   id="accidentFile"
                   ref="pondFile"
                   label-idle="<span>แตะที่นี่เพื่อแนบรูป</span>"
                   allow-multiple="true"
                   v-bind:allowFileEncode="true"
                   accepted-file-types="image/jpeg, image/png"
                   v-on:addfile="onAddFile"
                   v-on:removefile="onRemoveFile"
                   allowFileSizeValidation="true"
                   maxFileSize="7MB"
                   labelMaxFileSizeExceeded="รูปมีขนาดใหญ่เกินไป"
                   labelMaxFileSize="ขนาดของรูปภาพต้องไม่เกิน {filesize}" 
                   imagePreviewHeight="150"
                   />
    </div>

</template>

<script>
    // Import Vue FilePond
    import vueFilePond from 'vue-filepond'

    // Import image preview and file type validation plugins
    import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type'
    import FilePondPluginImagePreview from 'filepond-plugin-image-preview'
    import FilePondPluginFileEncode from 'filepond-plugin-file-encode';
    import FilePondPluginFileValidateSize from 'filepond-plugin-file-validate-size';
    import FilePondPluginImageResize from 'filepond-plugin-image-resize';

    // Create component
    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode, FilePondPluginFileValidateSize, FilePondPluginImageResize);

    export default {
        name: 'InputImg',
        props: [],
        components: {
            FilePond
        },
        data() {
            return {
                docs: []
            }
        },

        methods: {
            onRemoveFile: function (error, file) {
                this.docs = this.docs.filter((v) => {
                    return v.id !== file.id
                })
                console.log("del-", this.docs)
                this.$emit('storeFile', this.docs);
            },
            onAddFile: function (error, file) {
                if (file.fileSize < 7000000) {
                    var fileDataUrl = file.getFileEncodeDataURL()
                    var imgRes = new Image();
                    imgRes.src = fileDataUrl
                    var img = document.createElement("img");
                    img.onload = () => {
                        var canvas = document.createElement("canvas");
                        var MAX_WIDTH = 720;
                        var MAX_HEIGHT = 720;
                        var width = img.width;
                        var height = img.height;

                        if (width > height) {
                            if (width > MAX_WIDTH) {
                                height *= MAX_WIDTH / width;
                                width = MAX_WIDTH;
                            }
                        } else {
                            if (height > MAX_HEIGHT) {
                                width *= MAX_HEIGHT / height;
                                height = MAX_HEIGHT;
                            }
                        }
                        canvas.width = width;
                        canvas.height = height;
                        var ctx = canvas.getContext("2d");
                        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                        this.docs.push({ filename: file.filename, base64: canvas.toDataURL(file.fileType), id: file.id })
                    }
                    img.src = fileDataUrl
                    console.log('add', this.docs)
                    this.$emit('storeFile', this.docs);
                } else {
                    this.$refs.pondFile.removeFile(file.id)
                    if (error) {
                        this.$swal({
                            icon: 'error',
                            text: error.sub,
                            title: error.main,
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {
                               
                            }
                        })
                    }
                }

            },
            

        },

        mounted() {


        },
        created() {

        },

    }
</script>

<style scoped>
   
</style>
