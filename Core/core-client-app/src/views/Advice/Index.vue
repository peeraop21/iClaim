<template>

    <div class="container mt-2">

        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true"></loading>


        <div align="center" class="mt-4">
            <h2 id="header2">คำแนะนำ</h2>
            <br>
        </div>
        <!--<router-link class="btn-select-rights" :to="{ name: 'Rtc'}">RTC</router-link>-->
        <div hidden>
            <router-link class="btn-select-rights" :to="{ name: 'CameraRtc'}">newRTC</router-link>
            <button class="btn-select-rights" @click="showRtc=!showRtc">RTC</button>
            <button class="btn-select-rights" @click="showNewRtc=!showRtc">newRTC</button>
        </div>
        
        <div class="row mb-2" @click="active=!active" style="margin-top: -10px">
            <div class="col-12">
                <vs-card type="3" align="center">
                    <!-- <template #title>
                        <h3>Pot with a plant</h3>
                    </template>-->
                    <template #img>
                        <img src="@/assets/money.jpg" alt="">
                    </template>
                    <template #text>
                        <p style="font-weight: bold;">
                            สิทธิ์คุ้มครองตาม พ.ร.บ.
                        </p>
                    </template>
                </vs-card>
            </div>
        </div>
        <div class="row mb-2" @click="active2=!active2">
            <div class="col-12">
                <vs-card type="3" align="center">
                    <!-- <template #title>
                        <h3>Pot with a plant</h3>
                    </template>-->
                    <template #img>
                        <img src="@/assets/search.jpg" alt="">
                    </template>
                    <template #text>
                        <p style="font-weight: bold;">
                            ขั้นตอนการขอใช้สิทธิ์
                        </p>
                    </template>
                </vs-card>
            </div>
        </div>
        <div class="row mb-2" @click="active3=!active3">
            <div class="col-12">
                <vs-card type="3" align="center">
                    <!-- <template #title>
                        <h3>Pot with a plant</h3>
                    </template>-->
                    <template #img>
                        <img src="@/assets/id-card.jpg" alt="">
                    </template>
                    <template #text>
                        <p style="font-weight: bold;">
                            เงื่อนไขการใช้สิทธิ์
                        </p>
                        <p style="font-weight: bold;">
                            และเอกสารที่ต้องเตรียม
                        </p>
                    </template>
                </vs-card>
            </div>
        </div>

        <div align="center" class="mt-5">
            <h2 id="header2">กลุ่มผู้ใช้งานที่รองรับ</h2>
            <br>
        </div>
        <div class="row" style="margin-top: -15px">
            <div class="col-12 px-0" align="center">
                <vs-button circle
                           icon
                           light
                           flat
                           :active="active == 1"
                           @click="riderDialog=!riderDialog">
                    <img src="@/assets/rider.png" width="100">
                </vs-button>
                <p style="font-weight: bold; color: var(--main-color);">ผู้ขับขี่ที่เป็นผู้เอาประกันภัย</p>
            </div>
            <!--<div class="col-6 px-0" align="center">
                <vs-button circle
                           icon
                           light
                           flat
                           :active="active == 1"
                           @click="passangerDialog=!passangerDialog">
                    <img src="@/assets/passanger.png" width="100">
                </vs-button>
                <p style="font-weight: bold; color: var(--main-color);">ผู้โดยสารที่เป็นผู้ประสบภัยรถประกัน</p>
            </div>-->
        </div>
        <!-- Rider Dialog -->
        <vs-dialog width="550px" not-center v-model="riderDialog">
            <template #header>
                <h4 class="not-margin">
                    ผู้ขับขี่ที่เป็นผู้เอาประกันภัย
                </h4>
            </template>
            <div class="con-content" align="left">
                <p>ผู้ขับขี่ที่เป็นผู้เอาประกันภัย หมายถึง ผู้ขับขี่รถจักรยานยนต์คันที่ทำประกันภัยไว้ตาม พ.ร.บ. คุ้มครองผู้ประสบภัยจากรถ</p>
            </div>
        </vs-dialog>
        <!-- Passanger Dialog -->
        <vs-dialog width="550px" not-center v-model="passangerDialog">
            <template #header>
                <h4 class="not-margin">
                    ผู้โดยสารที่เป็นผู้ประสบภัยรถประกัน
                </h4>
            </template>
            <div class="con-content" align="left">
                <p>ผู้โดยสารที่เป็นผู้ประสบภัยรถประกัน หมายถึง ผู้ที่ซ้อนท้ายรถจักรยานยนต์คันที่ทำประกันภัยไว้ตาม พ.ร.บ. คุ้มครองผู้ประสบภัยจากรถ</p>
            </div>
        </vs-dialog>

        <div class="row mt-4 mb-2">
            <div class="col-12">
                <button class="button-next1" style="width: 100%; padding: 8px 0;" @click="checkRegisAgain">{{msg}}</button>
            </div>
            <br>
            <br>
            <br>
        </div>

        <!-- Dialog 1 -->
        <vs-dialog width="550px" not-center v-model="active">
            <template #header>
                <h4 class="not-margin">
                    สิทธิ์คุ้มครองตาม พ.ร.บ.
                </h4>
            </template>
            <div class="con-content" align="left">

                <label class="space-title"><strong class="purple-title">ค่าเสียหายเบื้องต้น</strong> ไม่ต้องรอผลพิสูจน์ว่าฝ่ายใดชนหรือฝ่ายใดเป็นฝ่ายถูกหรือผิด</label>
                <br />
                <strong>กรณีผู้ประสบภัยได้รับความเสียหายต่อร่างกาย</strong>
                <p>
                    บริษัทจะจ่ายค่ารักษาพยาบาล ตามจำนวนที่จ่ายไปจริง แต่ไม่เกิน30,000 บาทต่อหนึ่งคน
                </p>
                <strong>กรณีผู้ประสบภัยสูญเสียอวัยวะ/ทุพพลภาพอย่างถาวร</strong>
                <p>
                    บริษัทจะจ่ายค่าเสียหายเบื้องต้น จำนวน 35,000 บาทต่อหนึ่งคน
                </p>
                <strong>กรณีผู้ประสบภัยเสียชีวิต</strong>
                <p>
                    บริษัทจะจ่ายค่าปลงศพตามจำนวนเงินค่าเสียหายเบื้องต้น 35,000 บาทต่อหนึ่งคน
                </p>
                <br />
                <hr>

                <label class="space-title"><strong class="purple-title">ค่าสินไหมทดแทน</strong> คือ เงินชดเชยที่ฝ่ายถูกจะได้รับ หลังการพิสูจน์ผิดถูกแล้ว โดยได้รับความคุ้มครองรวมกับค่าเสียหายเบื้องต้นดังนี้</label>
                <br />
                <strong>กรณีได้รับความเสียหายต่อร่างกาย หรืออนามัย</strong>
                <p>
                    บริษัทจะจ่ายค่ารักษาพยาบาล ตามความเสียหายที่แท้จริงแต่ไม่เกิน80,000  บาทต่อหนึ่งคน
                </p>
                <strong>กรณีสูญเสียอวัยวะ/ทุพพลภาพอย่างถาวร หรือทุพพลภาพถาวรสิ้นเชิง</strong>
                <p>
                    บริษัทจะจ่ายค่าสินไหมทดแทน 200,000 - 500,000 บาทต่อหนึ่งคน
                </p>
                <strong>กรณีเสียชีวิต</strong>
                <p> บริษัทจะชดใช้ค่าสินไหมทดแทน 500,000  บาทต่อหนึ่งคน</p>
                <strong>กรณีที่เข้ารับการรักษาพยาบาลในสถานพยาบาล (คนไข้ใน)</strong>
                <p>
                    บริษัทจะจ่ายค่าชดเชยรายวัน วันละ 200 บาท (จำนวนรวมกันไม่เกิน 20 วัน)
                </p>

            </div>
        </vs-dialog>

        <!-- Dialog 2 -->
        <vs-dialog width="550px" not-center v-model="active2">
            <template #header>
                <h4 class="not-margin">
                    ขั้นตอนการขอใช้สิทธิ์
                </h4>
            </template>
            <div class="con-content" align="left">
                <p>1. ยืนยันตัวตนเพื่อขอใช้สิทธิ์ (ถ้าเคยยืนยันตัวตนแล้วจะข้ามขั้นตอนนี้ไป)</p>
                <p>2. กดเลือกแจ้งเหตุใหม่ (ถ้ามีรายการแจ้งเหตุแล้ว ท่านสามารถเลือกที่ใช้สิทธิ์เพิ่มเติมได้)</p>
                <p>3. เลือกประเภทการขอใช้สิทธิ์เบิกค่าเสียหายเบื้องต้น</p>
                <ul style="margin-top: -15px;">
                    <li>กรณีเบิกค่ารักษาพยาบาล</li>
                </ul>
                <p>4. กรอกข้อมูลคำขอใช้สิทธิ์ พร้อมแนบเอกสาร</p>
                <p>5. แนบรูปภาพหน้าสมุดบัญชีธนาคาร</p>
                <p>6. กดส่งคำร้อง พร้อมยืนยันการส่งด้วย SMS OTP</p>
                <p>7. ติดตามสถานะการดำเนินการของคำร้อง</p>
                <p>8. กดยืนยันจำนวนเงิน พร้อมยืนยันจำนวนเงินด้วย SMS OTP</p>
            </div>
        </vs-dialog>
        <!-- Dialog 3 -->
        <vs-dialog width="550px" not-center v-model="active3">
            <template #header>
                <h4 class="not-margin">
                    เงื่อนไขการใช้สิทธิ์ และเอกสารที่ต้องเตรียม
                </h4>
            </template>
            <div class="con-content" align="left">
                <label class="space-title-procedure"><strong class="purple-title">เงื่อนไขการใช้สิทธิ์ กรณีบาดเจ็บเบิกค่ารักษาพยาบาล</strong></label>
                <p>
                    1. ผู้ยื่นขอใช้สิทธิ์ต้องเป็น “ผู้ขับขี่ที่เป็นผู้เอาประกันภัย”
                    <br />
                    2. จำนวนเงินรวมไม่เกิน 2,000 บาท ต่อครั้ง
                    <br />
                    3. กรณีค่ารักษาเกิน 2,000 บาท สามารถติดต่อเข้ารับบริการได้ที่ บริษัทกลางคุ้มครองผู้ประสบภัยจากรถ จำกัด ทุกสาขาทั่วประเทศ  Call Center 1791
                </p>
                <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีบาดเจ็บเบิกค่ารักษาพยาบาล</strong></label>
                <br />
                <p>
                    1. สำเนาบัตรประจำตัวประชาชน
                    <br />
                    2. ใบเสร็จรับเงินค่ารักษาพยาบาล
                </p>
                <br />

            </div>
        </vs-dialog>
        <!-- Dialog RTC -->
        <b-modal v-model="showRtc"
                 hide-footer
                 hide-header
                 hide-header-close>
            <Rtc @base64="hiddenRtc" ref="rtc" class="text-center"></Rtc>
        </b-modal>
        <b-modal v-model="showNewRtc"
                 hide-footer
                 @hidden="hiddenRtc"
                 hide-header
                 hide-header-close>
            <camera-rtc class="text-center"></camera-rtc>
        </b-modal>
        <img style="width: -webkit-fill-available;" src="" id="img555">
        <!--<vs-dialog width="550px" center >
            <template #header>
                <h4 class="not-margin">
                    RTC
                </h4>
            </template>
            <div class="con-content" align="center">
                <Rtc></Rtc>

            </div>
        </vs-dialog>-->

    </div>
</template>
<script>

    import liff from '@line/liff'
    import axios from 'axios'
    // Import component
    import Loading from 'vue-loading-overlay';
    // Import stylesheet

    import Rtc from '../../components/WebRTC/Rtc.vue'
    import CameraRtc from '../../components/WebRTC/CameraRtc.vue'

    export default {
        name: 'Advice',
        components: {
            Loading,
            Rtc,
            CameraRtc
        },
        data() {
            return {
                msg: "ดำเนินการต่อ (ข้อมูลรับแจ้งเหตุ)",
                active: false,
                active2: false,
                active3: false,
                riderDialog: false,
                passangerDialog: false,
                page: 1,
                registered: null,
                isLoading: true,
                showRtc: false,
                showNewRtc: false
            }
        },
        methods: {
            hiddenRtc(value) {
                this.showRtc = false
                const previewImage1 = document.querySelector('#img555')
                previewImage1.src = value
            },
            getJwtToken() {
                var urlJwt = this.$store.state.envUrl + '/api/jwt'
                axios.post(urlJwt, {
                    Name: 'Nior',
                    Email: 'peeran@rvp.co.th'
                }).then((response) => {
                    this.$store.state.jwtToken = response.data
                    /*this.checkRegister()*/
                    this.isLoading = false
                }).catch(function (error) {
                    alert(error)
                })
            },
            checkRegister() {
                var url = this.$store.state.envUrl + '/api/user/CheckRegister/{userToken}'.replace('{userToken}', this.$store.state.userTokenLine);
                var tokenJwt = this.$store.state.jwtToken.token
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + tokenJwt
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        setTimeout(() => {
                            this.isLoading = false
                        }, 1000)
                        /*this.userApi = response.data;*/
                        this.registered = response.data;
                        if (this.registered == false) {
                            this.$swal({
                                icon: 'info',
                                text: 'เนื่องจากท่านพึ่งเข้าใช้งานครั้งแรก โปรดยืนยันตัวตนก่อนเข้าใช้งาน',
                                title: 'ขออภัย',
                                /*footer: '<a href="">Why do I have this issue?</a>'*/
                                showCancelButton: false,
                                showDenyButton: false,

                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {
                                    if (liff.getOS() == "ios") {
                                        this.$router.push('/Ocr')
                                    } else {
                                        liff.openWindow({
                                            url: location.origin + '/Ocr' + '?openExternalBrowser=1',
                                            external: true
                                        });
                                    }
                                }
                            })
                            /*this.$router.push({ name: 'Ocr' })*/
                        }

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            checkRegisAgain() {

                var url = this.$store.state.envUrl + '/api/user/CheckRegister/{userToken}'.replace('{userToken}', this.$store.state.userTokenLine);
                var tokenJwt = this.$store.state.jwtToken.token
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + tokenJwt
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        setTimeout(() => {
                            this.isLoading = false
                        }, 1000)
                        this.registered = response.data;
                        if (this.registered == false) {
                            this.$swal({
                                icon: 'info',
                                text: 'ท่านยังไม่ได้ทำการยืนยันตัวตน โปรดยืนยันตัวตนก่อนเข้าใช้งาน',
                                title: 'ขออภัย',
                                /*footer: '<a href="">Why do I have this issue?</a>'*/
                                showCancelButton: false,
                                showDenyButton: false,
                                confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                                confirmButtonColor: '#5c2e91',
                                willClose: () => {
                                    if (liff.getOS() == "ios") {
                                        this.$router.push('/Ocr')
                                    } else {
                                        liff.openWindow({
                                            url: location.origin + '/Ocr' + '?openExternalBrowser=1',
                                            external: true
                                        });
                                    }
                                }
                            })
                        } else {
                            this.$router.push('/Accident')
                        }
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        async created() {
            if (process.env.NODE_ENV == "production") {
                //--Publish--
                await liff.init({
                    liffId: this.$store.state.liffId,
                }).then(() => {
                    if (liff.isLoggedIn()) {
                        liff.getProfile().then(profile => {
                            this.$store.state.userTokenLine = profile.userId
                            this.getJwtToken()//ตรวจสอบการลงทะเบียน
                        }).catch(err => alert(err));
                    } else {
                        liff.login();
                        liff.getProfile().then(profile => {
                            this.$store.state.userTokenLine = profile.userId
                            this.getJwtToken()//ตรวจสอบการลงทะเบียน
                        }).catch(err => alert(err));
                    }
                }).catch(err => {
                    alert(err);
                    throw err
                });
            } else if (process.env.NODE_ENV == "development") {
                //--LocalHost--
                this.$store.state.userTokenLine = "Ub3ab146d6efded0db3dc7404f94966cb";
                this.getJwtToken()//ตรวจสอบการลงทะเบียน
            }




        },

    }


</script>
<style>
    .button-next1 {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 20px;
        font-size: 15px;
        border: none;
    }

    .bg-main {
        /*background-color: var(--main-color);
        background: -webkit-linear-gradient(rgb(114 60 177), rgb(144 96 199));*/
        margin-top: -25px;
    }
    /*CSS card and dialog*/
    .vs-card__img {
        width: 50%;
        height: 120px;
    }

    .con-footer {
        display: flex;
        align-items: center;
        justify-content: flex-end;
    }
    /*.col-6.px-0 {
        background-color: white;
    }*/
    .vs-button__content {
        padding: 10px 30px;
    }

    .vs-button {
        margin-left: 10px;
    }

    .not-margin {
        margin: 0px;
        font-weight: normal;
        padding: 10px;
        padding-bottom: 0px;
    }

    /*-----*/
    .purple-title {
        color: var(--main-color);
        font-size: 15px;
    }

    .space-title {
        margin: 1.0rem 0rem;
    }

    .space-title-procedure {
        margin: 0rem 0rem 0.1rem 0rem;
    }

    .card-advice-menu {
        border: 2px solid #5c2e91;
        padding: 7% 0px;
        width: 80%;
        margin: 2rem 1rem 2rem 1rem;
        border-radius: 10px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
    }

    .title-advice-menu {
        font-size: 15px;
        color: black;
        margin-bottom: 0px;
    }

    a[class="btn-intro"]:link, a[class="btn-intro"]:visited {
        background-color: white;
        color: black;
        border: 2px solid var(--main-color);
        padding: 7% 20px;
        width: 70%;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }

    a[class="btn-next"]:link, a[class="btn-next"]:visited {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }
</style>