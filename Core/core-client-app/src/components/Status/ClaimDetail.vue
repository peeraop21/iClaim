<template>
    <div class="container space-contianer" align="center">
        <h2 id="header2">รายละเอียดคำร้อง</h2>
        <div class="container">

            <!-- คน -->
            <div align="left" style="width: 100%;">
                <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลผู้ประสบภัย</label>
            </div>
            <div class="box-container mb-3">
                <div class="row">
                    <div class="col-9">
                        <p class="mb-0">ชื่อ-สกุล</p>
                        <div class="mt-0" v-if="accidentVictimData.prefix != null">
                            <p class="label-text">{{accidentVictimData.prefix}}{{accidentVictimData.fname}} {{accidentVictimData.lname}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.prefix === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-3">
                        <p class="mb-0">อายุ</p>
                        <div class="mt-0" v-if="accidentVictimData.age != null">
                            <p class="label-text">{{accidentVictimData.age}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.age === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-4">
                        <p class="mb-0">บ้านเลขที่</p>
                        <div class="mt-0" v-if="accidentVictimData.homeId != null">
                            <p class="label-text">{{accidentVictimData.homeId}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.homeId === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-2">
                        <p class="mb-0">หมู่</p>
                        <div class="mt-0" v-if="accidentVictimData.moo != null">
                            <p class="label-text">{{accidentVictimData.moo}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.moo === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ซอย</p>
                        <div class="mt-0" v-if="accidentVictimData.soi != null">
                            <p class="label-text">{{accidentVictimData.soi}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.soi === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">ถนน</p>
                        <div class="mt-0" v-if="accidentVictimData.road != null">
                            <p class="label-text">{{accidentVictimData.road}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.road === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ตำบล/แขวง</p>
                        <div class="mt-0" v-if="accidentVictimData.tumbolName != null">
                            <p class="label-text">{{accidentVictimData.tumbolName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.tumbolName === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">อำเภอ</p>
                        <div class="mt-0" v-if="accidentVictimData.districtName != null">
                            <p class="label-text">{{accidentVictimData.districtName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.districtName === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">จังหวัด</p>
                        <div class="mt-0" v-if="accidentVictimData.provinceName != null">
                            <p class="label-text">{{accidentVictimData.provinceName}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.provinceName === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">รหัสไปรษณีย์</p>
                        <div class="mt-0" v-if="accidentVictimData.zipcode != null">
                            <p class="label-text">{{accidentVictimData.zipcode}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="accidentVictimData.zipcode === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">เบอร์โทรศัพท์</p>
                        <div class="mt-0" v-if="userData.mobileNo != null">
                            <p class="label-text">{{userData.mobileNo}}</p>
                            <hr class="mt-0">
                        </div>
                        <div class="mt-0" v-else-if="userData.mobileNo === null">
                            <p class="label-text">-</p>
                            <hr class="mt-0">
                        </div>
                    </div>
                </div>
            </div>

            <!-- เหตุ -->
            <div align="left" style="width: 100%;">
                <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลอุบัติเหตุ</label>
            </div>
            <div class="box-container mb-3">

                <p class="mb-0">วันที่เกิดเหตุ</p>
                <div class="mt-0" v-if="accData.stringAccDate != null">
                    <p class="label-text">{{accData.stringAccDate}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accData.stringAccDate === null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">ลักษณะเกิดเหตุ</p>
                <div class="mt-0" v-if="accData.accNature!=''">
                    <p class="label-text">{{accData.accNature}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accData.accNature===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">สถานที่เกิดเหตุ</p>
                <div class="mt-0" v-if="accData.placeAcc!=''">
                    <p class="label-text">{{accData.placeAcc}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accData.placeAcc===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">หมายเลขทะเบียนรถคันเอาประกันภัย</p>
                <div class="mt-0" v-if="accidentCarData.foundCarLicense != ''">
                    <p class="label-text">{{accidentCarData.foundCarLicense}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accidentCarData.foundCarLicense ===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">เลขตัวถัง</p>
                <div class="mt-0" v-if="accidentCarData.foundChassisNo!=''">
                    <p class="label-text">{{accidentCarData.foundChassisNo}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accidentCarData.foundChassisNo===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                <div class="mt-0" v-if="accidentCarData.foundPolicyNo!=''">
                    <p class="label-text">{{accidentCarData.foundPolicyNo}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accidentCarData.foundPolicyNo===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>

            </div>

            <!-- เอกสาร -->
            <div align="left" style="width: 100%;">
                <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลเอกสารประกอบคำร้อง</label>
            </div>
            <div class="box-container mb-3">
                <p class="mb-0">อาการบาดเจ็บ</p>
                <div class="mt-0" v-if="bank!=null">
                    <p class="label-text">{{bank}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="bank===null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <!--<div class="card-bill" v-for="bill in bills" :key="bill.billNo">
                    <p class="mb-2">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                    <div v-if="bill.BillfileShow" align="center">
                        <img class="img-show" :src="bill.BillfileShow" />
                        <br />
                        <label>{{bill.filename}}</label>
                    </div>

                    <p class="mb-0">ชื่อโรงพยาบาล</p>
                    <div class="mt-0" v-if="bill.selectHospital!=''">
                        <p class="label-text">{{ bill.selectHospital }}</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bill.selectHospital===''">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <p class="mb-0">เลขที่ใบเสร็จ</p>
                            <div class="mt-0" v-if="bill.bill_no!=''">
                                <p class="label-text">{{bill.bill_no}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="bill.bill_no===''">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                        <div class="col-6">
                            <p class="mb-0">วันที่เข้ารักษา</p>
                            <div class="mt-0" v-if="bill.hospitalized_date!=''">
                                <p class="label-text">{{bill.hospitalized_date}}</p>
                                <hr class="mt-0">
                            </div>
                            <div class="mt-0" v-else-if="bill.hospitalized_date===''">
                                <p class="label-text">-</p>
                                <hr class="mt-0">
                            </div>
                        </div>
                    </div>
                    <p class="mb-0">จำนวนเงิน</p>
                    <div class="mt-0" v-if="bill.money!=''">
                        <p class="label-text">{{bill.money}} บาท</p>
                        <hr class="mt-0">
                    </div>
                    <div class="mt-0" v-else-if="bill.money===''">
                        <p class="label-text">-</p>
                        <hr class="mt-0">
                    </div>
                </div>-->

                <p class="mb-0">จำนวนเงินรวมทั้งหมด</p>
                <div class="mt-0" v-if="total_amount!=null">
                    <p class="label-text">{{total_amount}} บาท</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="total_amount===null">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
            </div>
            <!-- บัญชี -->
            <div align="left" style="width: 100%;">
                <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px; font-size: 25px"></ion-icon>
                <label align="left" class="title-advice-menu mb-1">ข้อมูลบัญชีรับเงิน</label>
            </div>
            <div class="box-container mb-3">
                <p class="mb-0">หน้าสมุดบัญชีธนาคาร</p>

                <!--<div v-if="bankFileDisplay" align="center">
                    <img class="img-show" :src="bankFileDisplay.base64" />
                    <br />
                    <label>{{bankFileDisplay.filename}}</label>
                </div>-->

                <p class="mb-0">ชื่อธนาคาร</p>
                <div class="mt-0" v-if="accountReceiveData.accountBankName!=''">
                    <p class="label-text">{{ accountReceiveData.accountBankName }}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accountReceiveData.accountBankName===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>

                <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                <div class="mt-0" v-if="accountReceiveData.accountName!=''">
                    <p class="label-text">{{accountReceiveData.accountName}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accountReceiveData.accountName===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>
                <p class="mb-0">เลขบัญชีธนาคาร</p>
                <div class="mt-0" v-if="accountReceiveData.accountNumber!=''">
                    <p class="label-text">{{accountReceiveData.accountNumber}}</p>
                    <hr class="mt-0">
                </div>
                <div class="mt-0" v-else-if="accountReceiveData.accountNumber===''">
                    <p class="label-text">-</p>
                    <hr class="mt-0">
                </div>

                <!-- <div class="form-check">
                <input class="form-check-input" type="checkbox" v-model="acceptClaim">
                <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                    ข้าพเจ้าตรวจสอบและยืนยันข้อมูลทุกอย่างเป็นความจริง
                </p>
                </div> -->
            </div>

        </div>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: "ClaimDetail",
        data() {
            return {
                bank: null,
                total_amount:null,
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
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
                    accountNumber:null


                }

            }
        },
        methods: {
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
                var url = '/api/Approval/DocumentReceive/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.$route.params.id).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
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
            await this.getAccidentCar();
            await this.getAccidentVictim();
            await this.getDocumentReceive();

        },
    }
</script>
<style>
  .label-text{
        margin-bottom:0px;

        color:gray;
    }
</style>