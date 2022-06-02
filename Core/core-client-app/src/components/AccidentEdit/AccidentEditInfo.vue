<template>
    <div>
        <div class="box-container shadow-box px-2 validate-me">
            <div class="accident-title fw-bold">ข้อมูลรายละเอียดอุบัติเหตุ</div>
            <div class="row">
                <div class="col-12">
                    <label class="form-label fw-bold">
                        วันที่เกิดเหตุ :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.accidentData.dateAccString}} เวลา {{init.accidentData.timeAcc}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-12">
                    <label class="form-label fw-bold">
                        ลักษณะการเกิดเหตุ :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.accidentData.accNature}}
                    </p>

                </div>
            </div>
            <div class="row mt-1">
                <div class="col-6">
                    <label class="form-label fw-bold">
                        สถานที่เกิดเหตุ :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.accidentData.accPlace}}
                    </p>
                </div>
                <div class="col-6">
                    <label class="form-label fw-bold">
                        ตำบอล/แขวง :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.accidentData.accSubDist}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-6">
                    <label class="form-label fw-bold">
                        อำเภอ/เขต :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.accidentData.accDist}}
                    </p>
                </div>
                <div class="col-6">
                    <label class="form-label fw-bold">
                        จังหวัด :
                    </label>
                    <p class="form-label p-under-line">
                        &nbsp;{{init.accidentData.accProv}}
                    </p>
                </div>
            </div>
            <div class="row mt-1">
                <div class="col-12">
                    <label class="form-label fw-bold">
                        รูปภาพสถานที่/สภาพแวดล้อม ที่เกิดเหตุ <label  v-if="init.checkNotPassTypes.length > 0 || init.checkNotPassComment">(เดิม)</label> :
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
            <div class="row mt-1"  v-if="init.checkNotPassTypes.length > 0 || init.checkNotPassComment">
                <div class="col-12">
                    <label for="accImagesInput" class="form-label fw-bold">รูปภาพสถานที่/สภาพแวดล้อม ที่เกิดเหตุ (ใหม่)<span class="star-require">*</span></label>
                    <InputImg ref="accImages" @storeFile="storeFile"></InputImg>
                    <div v-if="input.accImages.length == 0 && hasSubmit" class="un-input-image">
                        กรุณาแนบรูปสถานที่เกิดเหตุ.
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

    //import axios from 'axios'

    import InputImg from '../InputImgByFilePond/InputImg.vue'

    export default {
        name: 'AccidentEditInfo',
        props: ['hasSubmit'],
        components: {
            InputImg
        },
        data() {
            return {

                init: {
                    checkNotPassTypes: [],
                    checkNotPassComment: null,
                    accidentData: {},
                    images: []
                },
                input: {
                    accImages: [],
                },
                modalBigImage: false,
                srcBigImage: null,
            }
        },

        methods: {
            storeFile(file) {
                this.input.accImages = file
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
    .accident-title {
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
