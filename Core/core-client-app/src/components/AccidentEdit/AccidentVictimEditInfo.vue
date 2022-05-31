<template>
    <div>
        <div class="box-container shadow-box px-2 validate-me">
            <div class="accident-victim-title">ข้อมูลผู้ประสบภัย</div>
            <div class="row">
                <div class="col-12">
                    <label for="accVicBrokenImagesInput" class="form-label">รูปภาพอาการบาดเจ็บ<span class="star-require">*</span></label>
                    <InputImg ref="accVicBrokenImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accVicBrokenImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปอาการบาดเจ็บ.
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<script>

    /*import axios from 'axios'*/
    import InputImg from '../InputImgByFilePond/InputImg.vue'

    export default {
        name: 'AccidentVictimEditInfo',
        props: ['provinces', 'hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {
                init: {
                    districts: null,
                    subDistricts: null
                },
                input: {
                    accVicBrokenImages:[]
                },
            }
        },
        computed: {

        },
        methods: {
            storeFile(file) {
                this.input.accVicBrokenImages = file
            },
            

        },

        mounted() {


        },
        created() {
           
        },

    }
</script>

<style scoped>
    .accident-victim-title {
        text-align: center;
        font-size: 16px;
        text-decoration: underline;
        margin-bottom: 10px;
    }
    .form-label {
        margin-bottom: 0px;
    }
    textarea {
        height: 5rem;
    }
</style>
