<template>
    <div class="container space-contianer" align="center">
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
                                <p v-if="accidentCarData.foundPolicyNo!=''">เลขที่กรมธรรม์ : {{accidentCarData.foundPolicyNo}}</p>
                                <p v-else-if="accidentCarData.foundPolicyNo===''">เลขที่กรมธรรม์ : -</p>
                                <p v-if="hosData.stringRegDate!=''">วันที่ยื่นคำร้อง : {{hosData.stringRegDate}}</p>
                                <p v-else-if="hosData.stringRegDate===''">วันที่ยื่นคำร้อง : -</p>
                                <p v-if="hosData.appNo!=''" style="margin-bottom: 3px">ครั้งที่เรียกร้อง : {{hosData.appNo}}</p>
                                <p v-else-if="hosData.appNo===''" style="margin-bottom: 3px">ครั้งที่เรียกร้อง : -</p>
                            </div>
                        </div>
                        
                    </div>
                </div>
                <div class="col-12">
                    <div class="text-start">
                        <div v-if="total_amount!=null">
                            เอกสารฉบับนี้เป็นเอกสารยืนยันการรับเงินค่าเสียหายเบื้องต้น ตามเงื่อนไขกรมธรรม์คุ้มครองผู้ประสบภัยจากรถ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด
                            <span class="p-main-color">เป็นจำนวนเงิน {{total_amount}} บาท</span>
                            โดย {{userData.prefix}}{{userData.fname}} {{userData.lname}} มีความประสงค์รับเงินดังกล่าว
                        </div>
                        <div v-else-if="total_amount===null">
                            เอกสารฉบับนี้เป็นเอกสารยืนยันการรับเงินค่าเสียหายเบื้องต้น ตามเงื่อนไขกรมธรรม์คุ้มครองผู้ประสบภัยจากรถ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด
                            <span class="p-main-color">เป็นจำนวนเงิน - บาท</span>
                            โดย {{userData.prefix}}{{userData.fname}} {{userData.lname}} มีความประสงค์รับเงินดังกล่าว
                        </div>
                        
                    </div>
                    <div class="text-start mt-4 mb-4">
                        <div>
                            <p v-if="accountReceiveData.accountBankName!=''">โดยการโอนเงินเข้าบัญชีธนาคาร <span class="p-main-color"> {{ accountReceiveData.accountBankName }}</span></p>
                            <p v-else-if="accountReceiveData.accountBankName===''">โดยการโอนเงินเข้าบัญชีธนาคาร -</p>
                        </div>
                        <div style="margin-top: -15px">
                            <p v-if="accountReceiveData.accountName!=''">ชื่อบัญชีธนาคาร <span class="p-main-color"> {{ accountReceiveData.accountName }}</span></p>
                            <p v-else-if="accountReceiveData.accountName===''">ชื่อบัญชีธนาคาร -</p>
                        </div>
                        <div style="margin-top: -15px">
                            <p v-if="accountReceiveData.accountNumber!=''">เลขที่บัญชีธนาคาร <span class="p-main-color"> {{ accountReceiveData.accountNumber }}</span></p>
                            <p v-else-if="accountReceiveData.accountNumber===''">เลขที่บัญชีธนาคาร -</p>
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
                            <b-form-input class="mb-3" type="text" placeholder="xxx-xxx-9898" disabled />
                        </div>
                        <div class="col-5">
                            <button class="btn-request-otp" type="button">ขอรหัส OTP</button>
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
                            <p class=" pink-title fw-bold text-start "><ion-icon name="reload-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>ขอรหัสอีกครั้ง</p>
                        </div>
                        <div class="col-6  pt-2">
                            <p class=" black-title fw-bold text-start"><ion-icon name="time-outline" style="margin-bottom: -5px; padding-right: 5px; font-size: 20px"></ion-icon>00:20 น.</p>
                        </div>
                    </div>
                    <div>
                        <br>
                        <button class="btn-confirm-money" type="button" @click="showSwal">{{lblButton}}</button>
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
    import axios from 'axios'
    //Your Javascript lives within the Script Tag
    export default {
        name: "ConfirmMoney",
        data() {
            return {
                acceptR: false,
                lblButton: 'ยืนยันจำนวนเงิน',
                claimNo: '61/660/00337',
                money: '1,000',
                reason: '......',
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                bank: null,
                total_amount: null,
                //----Get AccidentCar
                accidentCarData: { foundCarLicense: '', foundChassisNo: '', foundPolicyNo: '' },
                //----Get AccidentCar
                accidentVictimData: {
                    accNo: null, victimNo: null, prefix: null, fname: null, lname: null, sex: null, age: null,
                    drvSocNo: null, homeId: null, moo: null, soi: null, road: null, tumbol: null, tumbolName: null,
                    district: null, districtName: null, province: null, provinceName: null, zipcode: null, telNo: null
                },
                accountReceiveData: {
                    accountBankName: null,
                    accountName: null,
                    accountNumber: null
                },
                // HosApp
                hosData: this.$store.getters.hosAppGetter(this.$route.params.id),
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
                        this.$router.push({ name: 'CheckStatus', params: { id: this.accData.stringAccNo } })
                    }
                })
            },
            handleOnComplete(value) {
                console.log('OTP completed: ', value);
            },
            handleOnChange(value) {
                console.log('OTP changed: ', value);
            },
            handleClearInput() {
                this.$refs.otpInput.clearInput();
            },
            getAccidentCar() {
                console.log('getAccidentCar');
                var url = '/api/Accident/Car/{accNo}/{channel}'.replace('{accNo}', this.accData.stringAccNo).replace('{channel}', this.accData.channel);
                axios.get(url)
                    .then((response) => {
                        this.accidentCarData = response.data;
                        console.log(this.accidentCarData);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getAccidentVictim() {
                console.log('getAccidentVictim');
                var mockIdcard = this.userData.idcardNo /*'3149900145384'*/;
                var url = '/api/Accident/Victim/{accNo}/{ch}/{userIdCard}'.replace('{accNo}', this.accData.stringAccNo).replace('{ch}', this.accData.channel).replace('{userIdCard}', mockIdcard);
                axios.get(url)
                    .then((response) => {
                        this.accidentVictimData = response.data;
                        console.log(this.accidentVictimData);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getDocumentReceive() {
                console.log('getDocumentReceive');
                var url = '/api/Approval/DocumentReceive/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.lastClaim.victimNo).replace('{appNo}', this.$route.params.appNo);
                axios.get(url)
                    .then((response) => {
                        this.accountReceiveData = response.data[0];
                        console.log(this.accountReceiveData);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
        },
        async mounted() {
            await console.log('accData', this.accData);
            console.log("hosApp: ", this.hosData);
            await this.getAccidentCar();
            await this.getAccidentVictim();
            await this.getDocumentReceive();
        }
        
        

    };
</script>
