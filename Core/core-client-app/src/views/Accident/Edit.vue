<template>
    <div class="container">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">
        </loading>
        <h2 id="header2">ข้อมูลอุบัติเหตุ</h2>
        <div class="row px-3">
            <p align="left" class="px-0" id="sub-header">&emsp;ผลการตรวจสอบข้อมูลอุบัติเหตุ ({{$route.params.accNo}}) ของท่านไม่ผ่าน เนื่องจาก</p>
            <div v-if="init.checkDetail.accCarImgCheckTypeName">
                <div class="mb-1"  align="left">
                    <u>ข้อมูลรถที่เกิดอุบัติเหตุ</u>
                </div>
                <div v-for="(item, index) in init.checkDetail.accCarImgCheckTypeName" :key="index">
                    <p v-if="item.id.endsWith('50')" class="ms-5 mb-1" align="left">{{index+1}}.&nbsp;{{init.checkDetail.accCarImgCheckComment}}</p>
                    <p v-if="!item.id.endsWith('50')" class="ms-5 mb-1" align="left">{{index+1}}.&nbsp;{{item.name}}</p>
                    <!--<p class="ms-5 mb-1" align="left">{{index+1}}{{item.name}}</p>-->
                </div>
            </div>
            <div v-if="init.checkDetail.accNatureImgCheckTypeName">
                <div class="mb-1" align="left">
                    <u>ข้อมูลอุบัติเหตุ</u>
                </div>
                <div v-for="(item, index) in init.checkDetail.accNatureImgCheckTypeName" :key="index">
                    <p v-if="item.id.endsWith('50')" class="ms-5 mb-1" align="left">{{index+1}}.&nbsp;{{init.checkDetail.accNatureImgCheckComment}}</p>
                    <p v-if="!item.id.endsWith('50')" class="ms-5 mb-1" align="left">{{index+1}}.&nbsp;{{item.name}}</p>
                </div>
            </div>
            <div v-if="init.checkDetail.accVictimImgCheckTypeName">
                <div class="mb-1" align="left">
                    <u>ข้อมูลผู้ประสบภัย</u>
                </div>
                <div v-for="(item, index) in init.checkDetail.accVictimImgCheckTypeName" :key="index">
                    <p v-if="item.id.endsWith('50')" class="ms-5 mb-1" align="left">{{index+1}}.&nbsp;{{init.checkDetail.accVictimImgCheckComment}}</p>
                    <p v-if="!item.id.endsWith('50')" class="ms-5 mb-1" align="left">{{index+1}}.&nbsp;{{item.name}}</p>
                </div>
            </div>
        </div>
        <div class="row">
            <AccidentCarEditInfo ref="AccidentCarEditInfo" class="mt-3" :hasSubmit="hasSubmit"></AccidentCarEditInfo>
            <AccidentEditInfo ref="AccidentEditInfo" class="mt-3" :hasSubmit="hasSubmit"></AccidentEditInfo>
            <AccidentVictimEditInfo ref="AccidentVictimEditInfo" class="mt-3" :hasSubmit="hasSubmit"></AccidentVictimEditInfo>
        </div>
        <button class="btn-next-submit mt-5 mb-3" @click="onSubmit" style="width: 100%; padding: 8px 0px;" type="button">ยืนยัน</button>
    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'

    import axios from 'axios'

    import AccidentEditInfo from '../../components/AccidentEdit/AccidentEditInfo.vue'
    import AccidentCarEditInfo from '../../components/AccidentEdit/AccidentCarEditInfo.vue'
    import AccidentVictimEditInfo from '../../components/AccidentEdit/AccidentVictimEditInfo.vue'

    // Import component
    import Loading from 'vue-loading-overlay';
    // Import stylesheet
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: 'AccidentEdit',
        components: {
            Loading,
            AccidentEditInfo,
            AccidentCarEditInfo,
            AccidentVictimEditInfo
        },
        mixins: [mixin],
        data() {
            return {
                init: {
                    checkDetail: {}
                },
                isLoading: true,
                formIsValid: false,
                hasSubmit: false,
            }
        },
        computed: {
        },
        methods: {
            storeData() {
                this.$store.state.inputAccidentData.AccidentInput = this.$refs.AccidentEditInfo.input
                this.$store.state.inputAccidentData.AccidentInput.isEditImage = (this.$refs.AccidentEditInfo.input.accImages.length > 0) ? true : false;
                this.$store.state.inputAccidentData.AccidentCarInput = this.$refs.AccidentCarEditInfo.input
                this.$store.state.inputAccidentData.AccidentCarInput.isEditImage = (this.$refs.AccidentCarEditInfo.input.accCarImages.length > 0) ? true : false;
                this.$store.state.inputAccidentData.AccidentVictimInput = this.$refs.AccidentVictimEditInfo.input
                this.$store.state.inputAccidentData.AccidentVictimInput.isEditImage = (this.$refs.AccidentVictimEditInfo.input.accVicBrokenImages.length > 0) ? true : false;
                this.$store.state.inputAccidentData.AccNo = this.$route.params.accNo
            },
            onSubmit() {

                this.hasSubmit = true
                this.storeData();

                if ((this.$refs.AccidentEditInfo.input.accImages.length > 0 || this.init.checkDetail.accNatureImgCheckType == 'Y')
                    && (this.$refs.AccidentCarEditInfo.input.accCarImages.length > 0 || this.init.checkDetail.accCarImgCheckType == 'Y')
                    && (this.$refs.AccidentVictimEditInfo.input.accVicBrokenImages.length > 0 || this.init.checkDetail.accVictimImgCheckType == 'Y')) {
                    this.formIsValid = true
                    this.$swal({
                        icon: 'question',
                        text: 'ท่านยืนยันที่จะส่งข้อมูลอุบัติเหตุเพิ่มเติม เพื่อให้เจ้าหน้าที่ตรวจสอบอีกครั้งหรือไม่?',
                        showCancelButton: false,
                        showDenyButton: true,
                        denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยกเลิก",
                        denyButtonColor: '#dad5e9',
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยืนยัน",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {

                        }
                    }).then((result) => {
                        if (result.isConfirmed) {
                            this.goToConfirmOTP();
                        }

                    });

                    console.log(this.formIsValid, this.$store.state.inputAccidentData)
                } else {
                    this.formIsValid = false
                    console.log(this.formIsValid, this.$store.state.inputAccidentData)
                }

            },
            goToConfirmOTP() {
                this.$router.push({ name: 'ConfirmOTP', params: { id: 'EditAccident', from: 'AccidentEdit' } })
            },
            getDataForThisPage() {
                var url = this.$store.state.envUrl + '/api/accident/DataForAccidentEditPage';
                const body = {
                    AccNo: this.$route.params.accNo,
                    UserIdCard: this.$store.state.userStateData.idcardNo,
                    VictimNo: this.$route.params.victimNo,
                };
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token,
                        'Content-Type': 'application/json',
                    }
                }
                axios.post(url, JSON.stringify(body), apiConfig)
                    .then((response) => {
                        this.$refs.AccidentCarEditInfo.init.carData = response.data.car
                        this.$refs.AccidentEditInfo.init.accidentData = response.data.accident
                        this.$refs.AccidentVictimEditInfo.init.victimData = response.data.victim
                        this.init.checkDetail = response.data.checkDetail
                        if (this.init.checkDetail.accCarImgCheckType != 'Y') {
                            this.$refs.AccidentCarEditInfo.init.checkNotPassTypes = this.init.checkDetail.accCarImgCheckTypeName
                            this.$refs.AccidentCarEditInfo.init.checkNotPassComment = this.init.checkDetail.accCarImgCheckComment
                        }
                        if (this.init.checkDetail.accNatureImgCheckType != 'Y') {
                            this.$refs.AccidentEditInfo.init.checkNotPassTypes = this.init.checkDetail.accNatureImgCheckTypeName
                            this.$refs.AccidentEditInfo.init.checkNotPassComment = this.init.checkDetail.accNutureImageCheckComment
                        }
                        if (this.init.checkDetail.accVictimImgCheckType != 'Y') {
                            this.$refs.AccidentVictimEditInfo.init.checkNotPassTypes = this.init.checkDetail.accVictimImgCheckTypeName
                            this.$refs.AccidentVictimEditInfo.init.checkNotPassComment = this.init.checkDetail.accVictimImgCheckComment
                        }
                        console.log(response)
                        this.isLoading = false
                    }).catch((error) => {
                        alert(error)
                    })
            },
            downloadFiles() {
                this.downloadFileFromECM(process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_FILE_SYSTEM_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_FILE_TEMPLATE_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_FILE_DOCUMENT_ID, this.$route.params.accNo)
                    .then((response) => {
                        if (response.data) {
                            for (let i = 0; i < response.data.length; i++) {
                                this.$refs.AccidentEditInfo.init.images.push('data:image/png;base64,' + response.data[i])
                            }
                        }
                    }).catch((error) => {
                        alert(error)
                    });
                this.downloadFileFromECM(process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_CAR_FILE_SYSTEM_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_CAR_FILE_TEMPLATE_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_CAR_FILE_DOCUMENT_ID, this.$route.params.accNo)
                    .then((response) => {
                        if (response.data) {
                            for (let i = 0; i < response.data.length; i++) {
                                this.$refs.AccidentCarEditInfo.init.images.push('data:image/png;base64,' + response.data[i])
                            }
                        }
                    }).catch((error) => {
                        alert(error)
                    })
                this.downloadFileFromECM(process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_VICTIM_FILE_SYSTEM_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_VICTIM_FILE_TEMPLATE_ID, process.env.VUE_APP_API_ECM_DOWNLOAD_ACCIDENT_VICTIM_FILE_DOCUMENT_ID, this.$route.params.accNo)
                    .then((response) => {
                        if (response.data) {
                            for (let i = 0; i < response.data.length; i++) {
                                this.$refs.AccidentVictimEditInfo.init.images.push('data:image/png;base64,' + response.data[i])
                            }
                        }
                    }).catch((error) => {
                        alert(error)
                    })
            },

        },
        async created() {
            await this.getJwtToken();
            this.getDataForThisPage()
            this.downloadFiles()

        },

    }
</script>

<style>
    #sub-header {
        font-size: 14px;
        font-weight: bold;
    }
</style>
