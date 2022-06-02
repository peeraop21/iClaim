<template>
    <div>
        <div class="box-container shadow-box px-2 validate-me">
            <div class="accident-car-title fw-bold">ข้อมูลรถที่เกิดอุบัติเหตุ</div>
            <div class="row mt-1">
                <div class="col-12">
                    <label class="form-label fw-bold">
                        ทะเบียน :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.carData.carLicense}} {{init.carData.carProvinceName}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-6">
                    <label class="form-label fw-bold">
                        รุ่น :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.carData.marque}}
                    </p>
                </div>
                <div class="col-6">
                    <label class="form-label fw-bold">
                        ขนาด :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.carData.engineSize}} cc
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-6">
                    <label class="form-label fw-bold">
                        เลขตัวถัง :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.carData.carTankNo}}
                    </p>
                </div>
                <div class="col-6">
                    <label class="form-label fw-bold">
                        เลขกรมธรรม์ :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.carData.policyNo}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-12">
                    <label class="form-label fw-bold">
                        รูปภาพรถที่เกิดเหตุ <label v-if="init.checkNotPassTypes.length > 0 || init.checkNotPassComment">(เดิม)</label> :
                    </label>
                    <div>
                        <img v-for="(item, index) in init.images" :key="index" class="img-show" :src="init.images[index]" @click="showBigImage(init.images[index])" />
                    </div>
                </div>
            </div>
            <div class="row mt-1"  v-if="init.checkNotPassTypes.length > 0 || init.checkNotPassComment">
                <div class="col-12">
                    <label class="mb-1" align="left" style="color:red">
                        &emsp;<u>เนื่องข้อมูลรถที่เกิดเหตุถูกพิจารณาให้ไม่ผ่านโดยเจ้าหน้าที่ กรุณาแก้ไขดังนี้</u>
                    </label>
                    <div v-for="(item, index) in init.checkNotPassTypes" :key="index">
                        <p v-if="item.id.endsWith('50')" class="ms-5 mb-1" align="left" style="color:red">{{index+1}}.&nbsp;{{init.checkNotPassComment}}</p>
                        <p v-if="!item.id.endsWith('50')" class="ms-5 mb-1" align="left" style="color:red">{{index+1}}.&nbsp;{{item.name}}</p>
                    </div>
                </div>
            </div>
            <div  class="row" v-if="init.checkNotPassTypes.length > 0 || init.checkNotPassComment"  >
                <div class="col-12">
                    <label for="accImagesInput" class="form-label fw-bold">รูปภาพรถที่เกิดเหตุ (ใหม่)<span class="star-require">*</span></label>
                    <InputImg ref="accImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accCarImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปรถที่เกิดเหตุ.
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">

                </div>
            </div>
        </div>
        <vs-dialog width="550px" not-center v-model="modalBigImage">
            <div class="con-content" align="center">
                <img class="big-img-show" :src="srcBigImage" />
            </div>
        </vs-dialog>
    </div>

</template>

<script>

    //import axios from 'axios'

    import InputImg from '../InputImgByFilePond/InputImg.vue'

    export default {
        name: 'AccidentCarEditInfo',
        props: ['hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {

                init: {
                    checkNotPassTypes: [],
                    checkNotPassComment: null,
                    carData: {},
                    images: []
                },
                input: {
                    accCarImages: [],
                },
                modalBigImage: false,
                srcBigImage: null,
            }
        },
        methods: {
            
            storeFile(file) {
                this.input.accCarImages = file
            },
            showBigImage(src) {
                this.modalBigImage = true
                this.srcBigImage = src
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

    textarea {
        height: 5rem;
    }

    .p-under-line {
        border-bottom: 1px solid black;
    }
</style>
