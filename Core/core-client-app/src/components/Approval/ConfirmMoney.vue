<template>
    <div class="container space-contianer" align="center">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <div class="container">
            <div class="row">
                <div class="col-12" align="center">
                    <h2 id="header2">ยืนยันจำนวนเงิน</h2>
                </div>
                <div class="col-12" align="center">
                    <div class="tab-user mt-4 mb-4">
                        <div class="col-12">
                            <div class="title-claim-number" style="margin-top: 0px; margin-bottom: 0px; line-height: 4px">
                                <p v-if="accData.accNo!=''">เลขที่รับแจ้ง : {{accData.accNo}}</p>
                                <p v-else-if="accData.accNo===''">เลขที่รับแจ้ง : -</p>
                                <p v-if="accData.lastClaim.claimNo!=''">เลขที่เคลม : {{accData.lastClaim.claimNo}}</p>
                                <p v-else-if="accData.lastClaim.claimNo===''">เลขที่เคลม : -</p>
                                <p v-if="confirmMoneyData.car.foundPolicyNo != null">เลขที่กรมธรรม์ : {{confirmMoneyData.car.foundPolicyNo}}</p>
                                <p v-else-if="confirmMoneyData.car.foundPolicyNo === null">เลขที่กรมธรรม์ : -</p>
                                <p v-if="confirmMoneyData.reqDate != null">วันที่ยื่นคำร้อง : {{confirmMoneyData.reqDate}} เวลา {{confirmMoneyData.reqTime}} น.</p>
                                <p v-else-if="confirmMoneyData.reqDate === null">วันที่ยื่นคำร้อง : -</p>
                                <p v-if="hosData.appNo!=''" style="margin-bottom: 3px">คำร้องขอที่ : {{hosData.appNo}}</p>
                                <p v-else-if="hosData.appNo===''" style="margin-bottom: 3px">คำร้องขอที่ : -</p>
                            </div>
                        </div>
                        
                    </div>
                </div>
                <div class="col-12">
                    <div class="text-start">
                        <div>
                            &emsp;&emsp;จากที่ท่านได้ทำการยื่นคำร้องขอเบิกค่ารักษาพยาบาลเป็นจำนวนเงิน <span class="p-main-color">{{confirmMoneyData.sumReqMoney}}</span> บาท มีบางรายการในใบเสร็จค่ารักษาจำเป็นต้องเปลี่ยนแปลง เพื่อให้ตรงตามเงื่อนไขของการเบิกค่ารักษาพยาบาลกรมธรรม์คุ้มครองผู้ประสบภัยจากรถ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด โดยมีรายละเอียดดังนี้
                        </div>
                        <div class="my-3" v-for="inv in confirmMoneyData.invoiceList" :key="inv.idInvhd">
                            <div class="div-center-image">
                                <div class="divImage" style="width:80%" v-if="inv.base64Image != null" align="left">
                                    <div class="row">
                                        <div class="col-5" align="center">
                                            <img class="img-show" :src="inv.base64Image" />
                                        </div>
                                        <div class="col-7 px-0">                                            
                                            <div class="row mt-2">
                                                <p class="inv-text">เล่มที่ใบเสร็จ : {{inv.bookNo}}</p>
                                                <p class="inv-text">เลขที่ใบเสร็จ : {{inv.receiptNo}}</p>
                                                <p class="inv-text">จำนวนเงินที่ร้องขอ : {{inv.reqMoney}} บาท</p>
                                                <p class="inv-text">จำนวนเงินที่จ่ายได้ : {{inv.payMoney}} บาท</p>
                                            </div>
                                        </div>                                       
                                    </div>
                                                                             
                                </div>
                            </div>
                        </div>
                        <div v-if="confirmMoneyData.sumReqMoney != null">
                            &emsp;&emsp;เอกสารฉบับนี้เป็นเอกสารยืนยันการรับเงินค่าเสียหายเบื้องต้น ตามเงื่อนไขกรมธรรม์คุ้มครองผู้ประสบภัยจากรถ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด เป็นจำนวนเงิน <span class="p-main-color">{{confirmMoneyData.sumPayMoney}}</span> บาท
                            
                            โดย {{userData.prefix}}{{userData.fname}} {{userData.lname}} มีความประสงค์รับเงินดังกล่าว
                        </div>
                        <!--<div v-else-if="total_amount===null">
                            &emsp;&emsp;เอกสารฉบับนี้เป็นเอกสารยืนยันการรับเงินค่าเสียหายเบื้องต้น ตามเงื่อนไขกรมธรรม์คุ้มครองผู้ประสบภัยจากรถ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด
                            <span class="p-main-color">เป็นจำนวนเงิน - บาท</span>
                            โดย {{userData.prefix}}{{userData.fname}} {{userData.lname}} มีความประสงค์รับเงินดังกล่าว
                        </div>-->
                        
                    </div>
                    <div class="text-start mt-4 mb-4">
                        <div>
                            <p v-if="confirmMoneyData.bankAccount.accountBankName != null">โดยการโอนเงินเข้าบัญชีธนาคาร <span class="p-main-color"> {{ confirmMoneyData.bankAccount.accountBankName }}</span></p>
                            <p v-else-if="confirmMoneyData.bankAccount.accountBankName === null">โดยการโอนเงินเข้าบัญชีธนาคาร -</p>
                        </div>
                        <div style="margin-top: -15px">
                            <p v-if="confirmMoneyData.bankAccount.accountName != null">ชื่อบัญชีธนาคาร <span class="p-main-color"> {{ confirmMoneyData.bankAccount.accountName }}</span></p>
                            <p v-else-if="confirmMoneyData.bankAccount.accountName === null">ชื่อบัญชีธนาคาร -</p>
                        </div>
                        <div style="margin-top: -15px">
                            <p v-if="confirmMoneyData.bankAccount.accountNumber != null">เลขที่บัญชีธนาคาร <span class="p-main-color"> {{ confirmMoneyData.bankAccount.accountNumber }}</span></p>
                            <p v-else-if="confirmMoneyData.bankAccount.accountNumber === null">เลขที่บัญชีธนาคาร -</p>
                        </div>
                    </div>
                </div>

            </div>
            <div class="row mb-2">
                <div class="col-2" style="padding-right: 0px; width:13%;">
                    <vs-checkbox v-model="acceptR" color="var(--main-color)">
                        <template #icon>
                            <i class='ti ti-check'></i>
                        </template>
                    </vs-checkbox>
                </div>
                <div class="col-10 px-0">
                    <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                        ข้าพเจ้าขอรับรองว่าข้อความทั้งหมดนี้มีความถูกต้องและเป็นความจริง ซึ่งเป็นไปตามความประสงค์ของข้าพเจ้า และเมื่อได้รับการโอนเงินเข้าบัญชีดังกล่าวภายใน 7 วัน นับจากวันที่ลงนามนี้แล้ว ให้ถือว่าข้าพเจ้าได้รับค่าเสียหายเบื้องต้น จากบริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด ครบถ้วนสมบูรณ์ทุกประการ
                    </p>
                </div>
            </div>
            <div class="row" v-if="acceptR">
                <div id="app" class="container ">
                    <p class="mt-4">
                        กรุณากดขอรหัส OTP เพื่อรับรหัสยืนยันจำนวนเงิน
                    </p>
                    <div class="row mb-4">
                        <div class="col-7">
                            <b-form-input class="mb-3" type="text" :placeholder="userData.mobileNo" disabled />
                        </div>
                        <div class="col-5">
                            <button class="btn-request-otp" @click="requestOTP" v-bind:disabled="disableBtnReqOTP" type="button">ขอรหัส OTP</button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <p class="space-title mt-4">
                                กรุณากรอกรหัส OTP ที่ส่งไปยัง SMS
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div style="display: flex; flex-direction: row; margin-left: 10px">
                                <v-otp-input ref="otpInput"
                                             input-classes="otp-input"
                                             separator="-"
                                             :num-inputs="6"
                                             :should-auto-focus="true"
                                             :is-input-num="true"
                                             @on-change="handleOnChange"
                                             @on-complete="handleOnComplete" />

                                <!--<button @click="handleClearInput()">Clear Input</button>-->
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6  pt-2">
                            <p class=" pink-title fw-bold text-start " @click="requestOTP" v-if="countDown == 60"><ion-icon name="reload-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>ขอรหัสอีกครั้ง</p>
                        </div>
                        <div class="col-6  pt-2">
                            <p class=" black-title fw-bold text-start"><ion-icon name="time-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>00:{{countDown}} น.</p>
                        </div>
                    </div>
                    <div>
                        <br>
                        <button class="btn-confirm-money" type="button" @click="submit">{{lblButton}}</button>
                        <button @click="postData">TestPostData</button>
                    </div>
                </div>
            </div>
        </div>
        <b-modal id="modal-1" title="BootstrapVue">
            <p class="my-4">Hello from modal!</p>
        </b-modal>
    </div>
</template>

<style>
    .inv-text {
        padding: 0px;
        margin-bottom: 2px;
        font-size: 12px;
    }
    .btn-confirm-money {
        background-color: var(--main-color);
        border: none;
        color: white;
        padding: 5px 20px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }
    .title-claim-number{
        font-weight:bold;
        font-size:12px;
        text-align:start;
    }
    .p-main-color{
        color: var(--main-color);
        font-weight:bold;
        font-size:13px;
        text-align:start;
    }

</style>

<script>
    import mixin from '../../mixin/index.js'
    import axios from 'axios'
    import qs from 'qs'
    //Your Javascript lives within the Script Tag
    // Import loading-overlay
    import Loading from 'vue-loading-overlay';
    import 'vue-loading-overlay/dist/vue-loading.css';

    export default {
        name: "ConfirmMoney",
        mixins: [mixin],
        components: {            
            Loading
        },
        data() {
            return {
                acceptR: false,
                lblButton: 'ยืนยันจำนวนเงิน',               
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                bank: null,
                total_amount: null,
                
                // HosApp
                hosData: this.$store.getters.hosAppGetter(this.$route.params.id, this.$route.params.appNo),
                countDown: 60,
                disableBtnReqOTP: false,
                inputOTP: null,
                confirmMoneyData: {
                    reqNo: null, reqDate: null, reqTime: null, sumReqMoney: null, sumPayMoney: null,
                    car: {
                        foundCarLicense: null, foundChassisNo: null, foundPolicyNo: null
                    },
                    bankAccount: {
                        accountBankName: null,
                        accountName: null,
                        accountNumber: null,
                        bankId: null
                    },
                    invoiceList: [{
                        idInvhd: null,
                        bookNo: null,
                        receiptNo: null,
                        reqMoney: null,
                        payMoney: null

                    }]
                },
                bankNames:null,
                isLoading:true,
            }
        },
        methods: {
            showSwal() {
                this.$swal({
                    icon: 'success',
                    text: 'บริษัทจะแจ้งวันที่โอนเงินให้ท่านทราบอีกครั้ง',
                    title: 'ยืนยันจำนวนเงินเรียบร้อย',
                    showCancelButton: false,
                    showDenyButton: false,
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                    }
                })
            },
            handleOnComplete(value) {
                console.log('OTP completed: ', value);
                this.inputOTP = value
            },
            handleOnChange(value) {
                console.log('OTP changed: ', value);
            },
            handleClearInput() {
                this.$refs.otpInput.clearInput();
            },
            
            getBankNames() {
                console.log('getBankNames');
                var url = this.$store.state.envUrl + '/api/Master/Bank';
                axios.get(url)
                    .then((response) => {
                        this.bankNames = response.data;
                        console.log(response.data);
                        this.getDataConfirmMoney();
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getDataConfirmMoney() {
                console.log('getDataConfirmMoney');
                var url = this.$store.state.envUrl + '/api/Approval/DataConfirmMoney/{accNo}/{victimNo}/{reqNo}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.victimNo).replace('{reqNo}', this.$route.params.appNo);
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        this.confirmMoneyData = response.data;
                        if (this.confirmMoneyData != null) {
                            for (let i = 0; i < this.bankNames.length; i++) {
                                if (this.confirmMoneyData.bankAccount.accountBankName == this.bankNames[i].bankCode) {
                                    this.confirmMoneyData.bankAccount.accountBankName = this.bankNames[i].name
                                    this.confirmMoneyData.bankAccount.bankId = this.bankNames[i].bankCode
                                    
                                }
                            }
                        }
                        for (let i = 0; i < this.confirmMoneyData.invoiceList.length; i++) {
                            this.confirmMoneyData.invoiceList[i].base64Image = 'data:image/png;base64,' + this.confirmMoneyData.invoiceList[i].base64Image
                        }
                        this.isLoading = false;
                        console.log("confirmData: ",this.confirmMoneyData)
                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.getDataConfirmMoney()
                        }
                    });
            },
            countDownTimer() {
                if (this.countDown > 0) {
                    this.disableBtnReqOTP = true
                    setTimeout(() => {
                        this.countDown -= 1
                        this.countDownTimer()
                    }, 1000)
                } else if (this.countDown <= 0) {
                    this.disableBtnReqOTP = false
                    this.countDown = 60

                }
            },
            postData() {
                var url = this.$store.state.envUrl + "/api/Approval/UpdateStatus/{accNo}/{victimNo}/{appNo}/{status}".replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo).replace('{status}','ConfirmMoney')
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        console.log(response);
                        if (response.data == "Success") {
                            this.$swal.close();
                            this.showSwalSuccess();
                        } else {
                            this.$swal.close();
                            this.showSwalError();
                        }
                        
                        
                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.postData()
                        }
                    });
            },
            requestOTP() {
                //ตัวจริง
                const url = "https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/OTP/RequestOTP";
                const body = {
                    TelNo: this.userData.mobileNo
                };
                //var tel = "";


                //ตัวเทส
                //const url = "https://smsotp.rvpeservice.com/OTP/RequestOTP";
                //const body = {
                //    ProjectName: "OTP_DigitalClaim",
                //    TelNo: this.userData.mobileNo
                //};
                console.log(qs.stringify(body))
                axios.post(url, qs.stringify(body), {
                    headers: {
                        // Overwrite Axios's automatically set Content-Type
                        'Content-Type': 'application/x-www-form-urlencoded',
                    }
                }).then((response) => {
                    this.countDownTimer()
                    this.dataOTP = response.data.result
                    console.log(this.dataOTP);

                }).catch((error) => {
                    this.$swal({
                        icon: 'error',
                        text: 'กรุณากดปุ่มขอรหัสยืนยัน OTP อีกครัง',
                        title: 'ผิดพลาด',
                        showCancelButton: false,
                        showDenyButton: true,
                        showConfirmButton: false,
                        denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        denyButtonColor: '#dad5e9'
                    });
                    console.log(error);
                });
            },

            submit: async function () {
                this.$swal({
                    title: 'กำลังตรวจสอบ',
                    html: 'ขณะนี้ระบบกำลังตรวจสอบรหัสยืนยัน OTP',
                    timerProgressBar: true,
                    didOpen: () => {
                        this.$swal.showLoading()

                    },
                    willClose: () => {

                    }
                })
                await this.verifyOTP()

            },
            verifyOTP() {
                //ตัวจริง
                const url = "https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/OTP/VerifyOTP";
                const body = {
                    'token': this.dataOTP.token,
                    'otp_code': this.inputOTP,
                    'ref_code': this.dataOTP.ref_code
                };


                //ตัวเทส
                //const url = "https://smsotp.rvpeservice.com/OTP/VerifyOTP";
                //const body = {
                //    'ProjectName': "OTP_DigitalClaim",
                //    'token': this.dataOTP.token,
                //    'otp_code': this.inputOTP,
                //    'ref_code': this.dataOTP.ref_code
                //};

                console.log("dataotp",qs.stringify(body))
                axios.post(url, qs.stringify(body), {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).then((response) => {
                    this.verifyResultOTP = response.data
                    console.log(this.verifyResultOTP.status);
                    if (this.verifyResultOTP.status == "false") {
                        this.$swal.close();
                        this.$swal({
                            icon: 'error',
                            text: 'รหัสยืนยัน OTP ไม่ถูกต้องกรุณาลองใหม่อีกครั้ง',
                            title: 'ผิดพลาด',
                            showCancelButton: false,
                            showDenyButton: true,
                            showConfirmButton: false,
                            denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            denyButtonColor: '#dad5e9',

                        })
                    } else if (this.verifyResultOTP.status == "true") {
                        this.postData()



                    }
                }).catch(function (error) {
                    console.log(error);
                });

            },
            showSwalError() {
                this.$swal({
                    icon: 'error',
                    text: 'เนื่องจากสถานะคำร้องของท่านไม่ได้อยู่ระหว่างการยืนยันจำนวนเงิน',
                    title: 'ไม่สามารถยืนยันจำนวนเงินได้',
                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                    showCancelButton: false,
                    showDenyButton: true,
                    denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    denyButtonColor: '#dad5e9',
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ติดตามสถานะ",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'Accident' })
                    }
                }).then((result) => {

                    if (result.isConfirmed) {
                        this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                    } else if (result.isDenied) {
                        this.$router.push({ name: 'Accident' })
                    }
                });
            },
            showSwalSuccess() {
                this.$swal({
                    icon: 'success',
                    text: 'บริษัทจะแจ้งวันที่โอนเงินให้ท่านทราบอีกครั้ง',
                    title: 'ยืนยันจำนวนเงินเรียบร้อย',
                    /*footer: '<a href="">Why do I have this issue?</a>'*/
                    showCancelButton: false,
                    showDenyButton: true,
                    denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    denyButtonColor: '#dad5e9',
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ติดตามสถานะ",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'Accident' })
                    }
                }).then((result) => {

                    if (result.isConfirmed) {
                        this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                    } else if (result.isDenied) {
                        this.$router.push({ name: 'Accident' })
                    }
                });
            },
        },
        async created() {
            //await console.log('accData', this.accData);
            //console.log("hosApp: ", this.hosData);
            //await this.getAccidentCar();
            //await this.getAccidentVictim();
            await this.getBankNames();
        }
        
        

    };
</script>
