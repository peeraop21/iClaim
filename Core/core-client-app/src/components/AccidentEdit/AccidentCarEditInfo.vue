<template>
    <div>
        <div class="box-container shadow-box px-2 validate-me">
            <div class="accident-car-title">ข้อมูลรถที่เกิดอุบัติเหตุ</div>

            <div class="row">
                <div class="col-12">
                    <label for="accImagesInput" class="form-label">รูปภาพสถานที่เกิดเหตุ<span class="star-require">*</span></label>
                    <InputImg ref="accImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปสถานที่เกิดเหตุ.
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<script>

    //import axios from 'axios'

    import InputImg from '../InputImgByFilePond/InputImg.vue'

    export default {
        name: 'AccidentCarEditInfo',
        props: ['provinces', 'hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {
              
                init: {
                    districts: null,
                    subDistricts:null
                },
                input: {
                    accImages:[]
                }
            }
        },

        methods: {
            storeFile(file) {
                console.log("file come: ", file)
                this.input.accImages = file
            },

        },

        mounted() {


        },
        created() {

        },

    }
</script>

<style scoped>
    .accident-car-title {
        text-align: center;
        font-size: 16px;
        text-decoration: underline;
        margin-bottom: 10px;
    }

    .form-label {
        margin-bottom: 0px;
    }
    textarea{
        height:5rem;
    }
</style>
