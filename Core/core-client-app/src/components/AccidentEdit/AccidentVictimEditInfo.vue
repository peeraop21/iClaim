<template>
    <div>
        <div class="box-container shadow-box px-2 validate-me">
            <div class="accident-victim-title fw-bold">ข้อมูลผู้ประสบภัย</div>
            <div class="row">
                <div class="col-8">
                    <label class="form-label fw-bold">
                        บ้านเลขที่ :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.homeId}}
                    </p>
                </div>
                <div class="col-4">
                    <label class="form-label fw-bold">
                        หมู่ที่ :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.moo}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-6">
                    <label class="form-label fw-bold">
                        ซอย :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.soi}}
                    </p>
                </div>
                <div class="col-6">
                    <label class="form-label fw-bold">
                        ถนน :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.road}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-6">
                    <label class="form-label fw-bold">
                        จังหวัด :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.provinceName}}
                    </p>
                </div>
                <div class="col-6">
                    <label class="form-label fw-bold">
                        อำเภอ/เขต :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.districtName}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-6">
                    <label class="form-label fw-bold">
                        ตำบล/แขวง :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.tumbolName}}
                    </p>
                </div>
                <div class="col-6">
                    <label class="form-label fw-bold">
                        ประเภทผู้ป่วย :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.victimType}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-12">
                    <label class="form-label fw-bold">
                        ลักษณะอาการบาดเจ็บ :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.victimData.detailBroken}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-12">
                    <label class="form-label fw-bold">
                        รูปภาพอาการบาดเจ็บ <label v-if="init.checkNotPassTypes.length > 0 || init.checkNotPassComment">(เดิม)</label> :
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
            <div class="row"  v-if="init.checkNotPassTypes.length > 0 || init.checkNotPassComment ">
                <div class="col-12">
                    <label for="accVicBrokenImagesInput" class="form-label fw-bold">รูปภาพอาการบาดเจ็บ (ใหม่)<span class="star-require">*</span></label>
                    <InputImg ref="accVicBrokenImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accVicBrokenImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปอาการบาดเจ็บ.
                    </div>
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

    /*import axios from 'axios'*/
    import InputImg from '../InputImgByFilePond/InputImg.vue'

    export default {
        name: 'AccidentVictimEditInfo',
        props: [ 'hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {
                init: {
                    checkNotPassTypes: [],
                    checkNotPassComment: null,
                    victimData: {},
                    images:[]
                },
                input: {
                    accVicUserLineId: this.$store.state.userTokenLine,
                    accVicBrokenImages: [],
                },
                modalBigImage: false,
                srcBigImage: null,
            }
        },
        computed: {

        },
        methods: {
            storeFile(file) {
                this.input.accVicBrokenImages = file
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
    .p-under-line {
        border-bottom: 1px solid black;
    }
</style>
