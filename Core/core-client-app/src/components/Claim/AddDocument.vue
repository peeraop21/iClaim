<template>
    <div class="container" align="center">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <h2 id="header2" class="mb-4">กรอกข้อมูล/แนบเอกสารเพิ่มเติม</h2>
        <div align="left">
            <p style="font-weight: bold">
                <ion-icon name="newspaper-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> เลขที่รับแจ้ง:
                <span class="p-main-color">{{ hosAppData.accNo }}</span> คำร้องที่:
                <span class="p-main-color">{{ hosAppData.appNo }}</span>
                <br>
                <ion-icon name="calendar-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> วันที่ยื่นคำร้อง:
                <span class="p-main-color">{{ hosAppData.stringRegDate }}</span>
                <br>
            </p>
        </div>
        <div class="tab-user mb-3">
            <div class="row">
                <div class="col-3 px-0">
                    <img src="@/assets/kid.png" width="70">
                </div>
                <div class="col-9 text-start px-2">
                    <span>ชื่อ-สกุล: <span style="color: var(--main-color)">{{userData.prefix}}{{userData.fname}} {{userData.lname}}</span></span>
                    <br>
                    <span>เบอร์โทรศัพท์: <span style="color: var(--main-color)">{{userData.mobileNo}}</span></span>
                    <br>
                    <span>สถานะ: <span style="color: var(--main-color)">ผู้ประสบภัยจากรถ</span></span>
                </div>
            </div>
        </div>
        <div align="left" class="mb-3">
            <p>
                มีความจำนงค์ในการติดต่อ บริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด เพื่อกระทำการ
                <span class="p-main-color"> ยื่นคำร้องขอรับเงินเบิกค่าเสียหายเบื้องต้น</span>
            </p>
            <p class="p-main-color">จำนวนเงิน {{hosAppData.sumMoney}} บาท</p>
            <p>
                จากกรมธรรม์ประกันภัยของบริษัท กลางคุ้มครองผู้ประสบภัยจากรถ จำกัด กรณีการเกิดอุบัติเหตุของ
                <br />
                รถหมายเลขทะเบียน
                <span v-for="(car, index) in accData.car" :key="`car-${index}`" class="p-main-color">{{car}} </span>
                <br />
                ในวันที่
                <span class="p-main-color"> {{accData.stringAccDate}}</span>
            </p>
            <p style="margin-bottom: 0px">โดยเอกสารหลักฐานที่ทำการยื่นมีความผิดพลาด ประกอบด้วยดังนี้</p>
            <div v-if="bookbankNotPassDescList.length > 0" style="padding-left:10px">
                <div style="color: var(--main-color)">
                    บัญชีธนาคาร
                    <div style="color: red; padding-left: 5px" v-for="(value, index) in bookbankNotPassDescList" :key="index">
                        - {{bookbankNotPassDescList[index]}}
                    </div>
                </div>
            </div>
            <div v-if="invNotPassDescList.length > 0" style="padding-left:10px">
                <div style="color: var(--main-color)" v-for="(input, indexi) in invNotPassDescList" :key="indexi">
                    ใบเสร็จที่ : {{indexi + 1}}
                    <div style="color: red; padding-left: 5px" v-for="(value, indexj) in invNotPassDescList[indexi]" :key="indexj">
                        - {{invNotPassDescList[indexi][indexj]}}
                    </div>
                </div>
            </div>


            <div hidden>
                <vs-checkbox color="var(--main-color)" v-model="invoiceDoc" style="margin-top: -15px" disabled>
                    <template #icon>
                        <i class='ti ti-check'></i>
                    </template>
                    <p style="margin-top: 15px; font-size: 13px">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                </vs-checkbox>
                <vs-checkbox color="var(--main-color)" v-model="accountDoc" style="margin-top: -35px" disabled>
                    <template #icon>
                        <i class='ti ti-check'></i>
                    </template>
                    <p style="margin-top: 15px; font-size: 13px">หน้าบัญชีธนาคาร</p>
                </vs-checkbox>
                <vs-checkbox color="var(--main-color)" v-model="doc2" style="margin-top: -35px" disabled>
                    <template #icon>
                        <i class='ti ti-check'></i>
                    </template>
                    <p style="margin-top: 15px; font-size: 13px">ใบรับรองแพทย์</p>
                </vs-checkbox>
                <vs-checkbox color="var(--main-color)" v-model="doc3" style="margin-top: -35px" disabled>
                    <template #icon>
                        <i class='ti ti-check'></i>
                    </template>
                    <p style="margin-top: 15px; font-size: 13px">สำเนาบันทึกประจำวันของพนักงานสอบสวน</p>
                </vs-checkbox>
            </div>

        </div>
        <vs-dialog width="550px" not-center v-model="modalBigImage">
            <div class="con-content" align="center">
                <img class="big-img-show" :src="srcBigImage" />
            </div>
        </vs-dialog>


        <div v-if="doc2 || doc3">
            <div align="left">
                <label>เอกสารประกอบคำร้องกรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
            </div>
            <div class="box-container">
                <div v-if="doc2">
                    <p class="mb-0">ใบรับรองแพทย์</p>
                    <div>
                        <file-pond name="idCardFile"
                                   ref="pond"
                                   label-idle="กดที่นี่เพื่ออัพโหลดใบรับรองแพทย์"
                                   credits="null"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-bind:files="idCardFile"
                                   v-on:addfile="onAddIdCardFile" />
                    </div>
                </div>
                <div v-if="doc3">
                    <p class="mb-0">สำเนาบันทึกประจำวันของพนักงานสอบสวน</p>
                    <div>
                        <file-pond name="diaryFile"
                                   ref="pond"
                                   label-idle="กดที่นี่เพื่ออัพโหลดสำเนาบันทึกประจำวัน"
                                   credits="null"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-bind:files="diaryFile"
                                   v-on:addfile="onAddDiaryFile" />
                    </div>
                </div>
            </div>
            <br />
        </div>

        <div v-if="accountDoc">


            <div class="box-container">
                <div align="left">
                    <label>หน้าบัญชีธนาคารรับเงิน</label>
                </div>
                <div class="form-group ">
                    <div v-if="!displayBankAccount.isDisabledInputImageBookBank">
                        <file-pond name="bankFile"
                                   ref="pond"
                                   credits="null"
                                   label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร1"
                                   v-bind:allow-multiple="false"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-on:addfile="onAddBankAccountFile"
                                   v-if="inputBank.isEditBankImage" />
                    </div>


                    <div v-if="!displayBankAccount.isDisabledImageBookBank">
                        <div class="mt-2 mb-2" v-if="!inputBank.isEditBankImage" align="center" @click="showBigImage(bankFileDisplay.base64)">
                            <div class="div-center-image">
                                <div class="divImage">
                                    <img class="img-show" :src="bankFileDisplay.base64" />
                                    <br />
                                    <label></label>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
                <button v-if="!displayBankAccount.isDisabledInputImageBookBank" class="btn-change-bank" @click="changeAccountBank">{{inputBank.displayBtnChangeBankImage}}</button>

                <p class="mb-0 px-2">ชื่อธนาคาร</p>
                <div class="mb-2">
                    <select :disabled="displayBankAccount.isDisabledBankName" v-model="$v.inputBank.accountBankName.$model" style="font-size: 13px;" :class="{ 'is-invalid': $v.inputBank.accountBankName.$error }">
                        <option v-for="bankName in bankNames" :value="bankName.name" v-bind:key="bankName.bank" style="font-size: 12px; line-height: 0px">
                            {{ bankName.name }}
                        </option>
                    </select>
                    <div v-if="submitted && !$v.inputBank.accountBankName.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกบัญชีธนาคาร</div>
                </div>
                <p class="mb-0 px-2">ชื่อบัญชีธนาคาร</p>
                <b-form-input :disabled="displayBankAccount.isDisabledAccountName" class="mt-0 mb-2" v-model="$v.inputBank.accountName.$model" :class="{ 'is-invalid': $v.inputBank.accountName.$error }"></b-form-input>
                <div v-if="submitted && !$v.inputBank.accountName.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกชื่อบัญชีธนาคาร</div>
                <p class="mb-0 px-2">เลขบัญชีธนาคาร</p>
                <b-form-input :disabled="displayBankAccount.isDisabledAccountNumber" class="mt-0 mb-2" v-model="$v.inputBank.accountNumber.$model" :class="{ 'is-invalid': $v.inputBank.accountNumber.$error }"></b-form-input>
                <div v-if="submitted && !$v.inputBank.accountNumber.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขที่บัญชี</div>
                <br>
            </div>
        </div>
        <div v-if="invoiceDoc">

            <br />
            <div class="box-container mt-2" v-for="(input, index) in $v.bills.$each.$iter" :key="index">
                <div class="input wrapper flex items-center">
                    <p class="px-2 mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาลที่ : {{calIndex(index)}}</p>
                    <!--<input type="file" @change="onFileChange">-->
                    <div v-if="!displayBills[index].isDisabledInputImageBill">
                        <file-pond credits="null"
                                   label-idle="กดที่นี่เพื่ออัพโหลดใบเสร็จค่ารักษา"
                                   v-bind:allow-multiple="false"
                                   v-bind:allowFileEncode="true"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-if="bills[index].isEditImage"
                                   v-bind:files="bills[index].file"
                                   v-model="bills[index].file"
                                   v-on:addfile="onAddBillFile(index)" />
                    </div>
                    <div v-if="!displayBills[index].isDisabledImageBill">
                        <div class="mt-2 mb-2" v-if="!bills[index].isEditImage" align="center">
                            <div class="div-center-image" @click="showBigImage(bills[index].billFileShow)">
                                <div class="divImage">
                                    <img class="img-show" :src="bills[index].billFileShow" />
                                    <br />
                                    <label>{{bills[index].filename}}</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <button v-if="!displayBills[index].isDisabledInputImageBill" class="btn-change-bank " @click="changeBillImage(index)">{{bills[index].displayBtnChangeBillImage}}</button>
                    <vs-dialog width="550px" not-center v-model="modalHospital">
                        <template #header>
                            <h4 class="not-margin">
                                เลือกโรงพยาบาล
                            </h4>
                        </template>
                        <div class="con-content" align="left">
                            <div class="d-block text-left">
                                <div class="mb-2">
                                    <label class="px-2">จังหวัด</label>
                                    <select name="category" id="category" v-model="selectChangwat" @change="onChangwatChange">
                                        <option v-for="(category, index) in changwats" :value="category.changwatshortname" :key="index" style="font-size: 12px; line-height: 0px">
                                            {{ category.changwatname }}
                                        </option>
                                    </select>
                                </div>
                                <div class="mb-2" v-show="divHospitalModal">
                                    <label class="px-2">โรงพยาบาล</label>
                                    <select name="item" id="item" v-model="mockHospital">
                                        <option v-for="(item, index) in filteredItems" :value="item.HOSPITALNAME " :key="index">
                                            {{ item.HOSPITALNAME  }}
                                        </option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <template #footer>
                            <div class="footer-dialog">
                                <vs-button block @click="submitModalHospital(index)">
                                    ยืนยัน
                                </vs-button>

                            </div>
                        </template>
                    </vs-dialog>
                    <vs-dialog width="550px" not-center v-model="modalWounded">
                        <template #header>
                            <h4 class="not-margin">
                                เลือกอาการบาดเจ็บ
                            </h4>
                        </template>
                        <div class="con-content" align="left">
                            <div class="d-block text-left">
                                <div class="mb-2">
                                    <label class="px-2">ประเภท</label>
                                    <select name="category" id="category" v-model="selectOrgan" @change="onOrganChange">
                                        <option v-for="(category, index) in organ" :value="category" :key="index" style="font-size: 12px; line-height: 0px">
                                            {{ category }}
                                        </option>
                                    </select>
                                </div>
                                <div class="mb-2" v-show="divWoundedModal">
                                    <label class="px-2">อาการบาดเจ็บ</label>
                                    <select name="item" id="item" v-model="mockWounded">
                                        <option v-for="(item, index) in filteredWoundedItems" :value="item.wounded" :key="index">
                                            {{ item.wounded  }}
                                        </option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <template #footer>
                            <div class="footer-dialog">
                                <vs-button block @click="submitModalWounded(index)">
                                    ยืนยัน
                                </vs-button>

                            </div>
                        </template>
                    </vs-dialog>
                    <div v-if="!displayBills[index].isHideFormInput">
                        <label class="px-2">อาการบาดเจ็บ</label>
                        <b-form-input :disabled="displayBills[index].isDisabledConsider" class="mt-0 mb-2" v-model="input.injuri.$model" placeholder="" type="text" @click="modalWounded=!modalWounded" :class="{ 'is-invalid': input.injuri.$error }"></b-form-input>
                        <div v-if="submitted && !input.injuri.required" class="invalid-feedback" style="margin-top: -5px;">กรุณาเลือกอาการบาดเจ็บ</div>
                        <label class="px-2">ประเภทผู้ป่วย</label>
                        <br />
                        <div class="mt-1 mb-2" style="float: left;">
                            <vs-radio :disabled="displayBills[index].isDisabledVictimType" v-model="input.typePatient.$model" val="IPD" style="float: left">
                                ผู้ป่วยใน (นอนพักรักษาตัวในโรงพยาบาลตั้งแต่ 6 ชั่วโมงขึ้นไป)
                            </vs-radio>
                            <vs-radio :disabled="displayBills[index].isDisabledVictimType" v-model="input.typePatient.$model" val="OPD" class="mt-1" style="float: left">
                                ผู้ป่วยนอก (ไม่ได้นอนพักรักษาตัวในโรงพยาบาล)
                            </vs-radio>

                        </div>
                        <label class="px-2">โรงพยาบาล</label>
                        <b-form-input :disabled="displayBills[index].isDisabledHospital" class="mt-0 mb-2" v-model="input.selectHospital.$model" type="text" @click="modalHospital=!modalHospital" :class="{ 'is-invalid': input.selectHospital.$error }" />
                        <div v-if="submitted && !input.selectHospital.required" class="invalid-feedback" style="margin-top: -5px;">กรุณาเลือกโรงพยาบาล</div>
                        <div class="row">
                            <div class="col-6">
                                <label class="px-2">ใบเสร็จเล่มที่</label>
                                <b-form-input :disabled="displayBills[index].isDisabledBookNo" class="mt-0 mb-2" v-model="input.bookNo.$model" type="text" placeholder="" :class="{ 'is-invalid': input.bookNo.$error }" />
                                <div v-if="submitted && !input.bookNo.required" class="invalid-feedback" style="margin-top: -5px;">กรุณากรอกเล่มที่ใบเสร็จ</div>
                            </div>
                            <div class="col-6">
                                <label class="px-2">เลขที่ใบเสร็จ</label>
                                <b-form-input :disabled="displayBills[index].isDisabledReceiptNo" class="mt-0 mb-2" v-model="input.bill_no.$model" type="text" placeholder="" :class="{ 'is-invalid': input.bill_no.$error }" />
                                <div v-if="submitted && !input.bill_no.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขที่ใบเสร็จ</div>
                            </div>
                        </div>
                        <label class="px-2">จำนวนเงิน</label>
                        <b-form-input :disabled="displayBills[index].isDisabledMoney" class="mt-0 mb-2" v-model="input.money.$model" type="number" placeholder="" @change="calMoney" :class="{ 'is-invalid': input.money.$error }" />
                        <div v-if="submitted && !input.money.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกจำนวนเงิน</div>
                        <div class="row">
                            <div class="col-8">
                                <label class="px-2">วันที่เข้ารักษา</label>
                            </div>
                            <div class="col-4">
                                <label class="px-2">เวลา</label>
                            </div>
                        </div>

                        <div class="row">
                            <v-date-picker v-model="input.hospitalized_date.$model" class="flex-grow col-8" locale="th" mode="date" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                                <template v-slot="{ inputValue, inputEvents }">
                                    <input :disabled="displayBills[index].isDisabledTakenDate" class=" mt-0 mb-2 form-control "
                                           :value="inputValue"
                                           v-on="inputEvents"
                                           :class="{ 'is-invalid': input.hospitalized_date.$error }"
                                           readonly />
                                    <div v-if="submitted && !input.hospitalized_date.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกวันที่เข้ารักษา</div>
                                </template>
                            </v-date-picker>
                            <v-date-picker v-model="input.hospitalized_time.$model" class="flex-grow col-4" locale="th" mode="time" is24hr :model-config="timeModelConfig" :dayPopover="{}">
                                <template v-slot="{ inputValue, inputEvents }">
                                    <input :disabled="displayBills[index].isDisabledTakenTime" class=" mt-0 mb-2 form-control "
                                           :value="inputValue"
                                           v-on="inputEvents"
                                           :class="{ 'is-invalid': input.hospitalized_time.$error }"
                                           readonly />
                                    <div v-if="submitted && !input.hospitalized_time.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกเวลาที่เข้ารักษา</div>
                                </template>
                            </v-date-picker>
                        </div>

                        <div v-if="input.typePatient==='IPD'">
                            <div class="row">
                                <div class="col-8">
                                    <label class="px-2">วันที่ออกจากโรงพยาบาล</label>
                                </div>
                                <div class="col-4">
                                    <label class="px-2">เวลา</label>
                                </div>
                            </div>
                            <div class="row">
                                <v-date-picker v-model="input.out_hospital_date.$model" class="flex-grow col-8" locale="th" :max-date='new Date()' :attributes='attrs' :model-config="dateModelConfig">
                                    <template v-slot="{ inputValue, inputEvents }">
                                        <input :disabled="displayBills[index].isDisabledDispenseDate" class=" mt-0 mb-2 form-control "
                                               :value="inputValue"
                                               v-on="inputEvents"
                                               :class="{ 'is-invalid': input.out_hospital_date.$error }"
                                               readonly />
                                        <div v-if="submitted && !input.out_hospital_time.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกวันที่ที่ออกจากโรงพยาบาล</div>
                                    </template>
                                </v-date-picker>
                                <v-date-picker v-model="input.out_hospital_time.$model" class="flex-grow col-4" locale="th" mode="time" is24hr :model-config="timeModelConfig">
                                    <template v-slot="{ inputValue, inputEvents }">
                                        <input :disabled="displayBills[index].isDisabledDispenseTime" class=" mt-0 mb-2 form-control "
                                               :value="inputValue"
                                               v-on="inputEvents"
                                               :class="{ 'is-invalid': input.out_hospital_time.$error }"
                                               readonly />
                                        <div v-if="submitted && !input.out_hospital_time.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกเวลาที่ออกจากโรงพยาบาล</div>
                                    </template>
                                </v-date-picker>
                            </div>

                        </div>
                    </div>
                    <br>


                </div>
            </div>
            <br>
            <p class="p_right">รวมจำนวนเงินที่ขอเบิก: {{ total_amount }} บาท</p>
            <br>
        </div>
        <br />
        <br />
        <button class="btn-confirm-money" type="button" @click="submit">ส่งเอกสารเพิ่มเติม</button>
        <br>
        <br>
        <br>
    </div>
</template>

<script>
    import axios from 'axios'
    import * as moment from "moment/moment";
    //Your Javascript lives within the Script Tag

    // Import loading-overlay
    import Loading from 'vue-loading-overlay';

    import { required } from "vuelidate/lib/validators";

    export default {
        name: "AddDocument",
        components: {
            Loading
        },
        data() {
            return {
                attrs: [
                    {
                        key: 'today',
                        dot: true,
                        dates: new Date(),
                    },
                ],
                dateModelConfig: {
                    type: 'string',
                    mask: 'YYYY-MM-DD',
                },
                timeModelConfig: {
                    type: 'string',
                    mask: 'HH:mm',
                },
                msg: 'ดำเนินการต่อ',
                idCardFile: null,
                idCardFileDisplay: { file: null, filename: "", base64: "" },
                diaryFile: null,
                diaryFileDisplay: { file: null, filename: "", base64: "" },
                // ---Bill

                injuri: '',
                typePatient: 0,
                typeContect: 1,
                bills: [{
                    billNo: 1, bill_no: "", bookNo: "", selectHospital: '', money: "", hospitalized_date: "", hospitalized_time: ""
                    , out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: ""
                    , file: null, billFileShow: "", filename: "", editBillImage: "", isEditImage: false, displayBtnChangeBillImage: "แก้ไขรูปใบเสร็จ"
                }],
                total_amount: 0,
                // --BookBank
                inputBank: { accountName: '', accountNumber: '', accountBankName: '', bankId: '', bankBase64String: '', bankFilename: '', isEditBankImage: false, displayBtnChangeBankImage: '' },
                bank: '',
                phoneNumbers: [{ phone: "" }],
                image: '',
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                formType: this.$route.params.type,
                billsFile: null,
                bankFile: null,
                bankFileDisplay: { file: null, filename: "", base64: "" },
                // ----Dialog
                active: false,
                dialogHospital: false,
                // Radio in Dialog
                picked: 1,
                //----Get Bank Name
                invoicehd: [],
                //----Get Bank Name
                bankNames: [],
                selectBank: {
                    bankName: ''
                },
                //----Get Hospital Name
                hospitals: [],
                //selectHospital: '',
                //----Get Changwat Name
                changwats: [],
                selectChangwat: '',
                modalHospital: false,
                mockHospital: '',
                divHospitalModal: false,
                //----Get Wounded
                wounded: [],
                organ: [],
                mockWounded: '',
                modalWounded: false,
                selectWounded: '',
                selectOrgan: '',
                divWoundedModal: false,
                //----Get Wounded
                documentCheck: [],
                // HosApp
                hosAppData: this.$store.getters.hosAppGetter(this.$route.params.id, this.$route.params.appNo),
                // Document Checkbox
                invoiceDoc: false,
                doc2: false,
                doc3: false,
                accountDoc: false,
                isLoading: true,
                displayBankAccount: { isDisabledInputImageBookBank: true, isDisabledImageBookBank: true, isDisabledBankName: true, isDisabledAccountName: true, isDisabledAccountNumber: true },
                displayBills: [{
                    isDisabledInputImageBill: true, isDisabledImageBill: true, isDisabledConsider: true, isDisabledVictimType: true, isDisabledHospital: true,
                    isDisabledBookNo: true, isDisabledReceiptNo: true, isDisabledMoney: true, isDisabledTakenDate: true, isDisabledTakenTime: true,
                    isDisabledDispenseDate: true, isDisabledDispenseTime: true, isHideFormInput: true
                }],
                submitted: false,
                modalBigImage: false,
                srcBigImage: null,
                invNotPassDescList: [],
                bookbankNotPassDescList: []


            };
        },
        validations() {
            if (this.bills.some(s => s.typePatient === "IPD")) {
                return {
                    bills: {
                        required,
                        $each: {
                            injuri: { required },
                            typePatient: { required },
                            selectHospital: { required },
                            bookNo: { required },
                            bill_no: { required },
                            money: { required },
                            hospitalized_date: { required },
                            hospitalized_time: { required },
                            out_hospital_date: { required },
                            out_hospital_time: { required }

                        }
                    },
                    inputBank: {
                        accountName: { required },
                        accountNumber: { required },
                        accountBankName: { required }
                    },
                }

            } else {
                return {
                    bills: {
                        required,
                        $each: {
                            injuri: { required },
                            typePatient: { required },
                            selectHospital: { required },
                            bookNo: { required },
                            bill_no: { required },
                            money: { required },
                            hospitalized_date: { required },
                            hospitalized_time: { required },


                        }
                    },
                    inputBank: {
                        accountName: { required },
                        accountNumber: { required },
                        accountBankName: { required }
                    },
                }
            }
        },
        methods: {
            onOrganChange() {

                this.divWoundedModal = true;

            },
            changeAccountBank() {
                console.log("ChangeBank")
                if (!this.inputBank.isEditBankImage) {
                    console.log("ChangeBank1", this.inputBank)

                    this.inputBank.isEditBankImage = true
                    this.inputBank.displayBtnChangeBankImage = "ใช้รูปบัญชีเดิม"
                } else {
                    this.inputBank.isEditBankImage = false
                    this.inputBank.displayBtnChangeBankImage = "แก้ไขรูปบัญชีรับเงิน"
                }

            },
            changeBillImage(index) {
                if (this.bills[index].isEditImage == false) {
                    this.displayBills[index].isHideFormInput = false
                    this.bills[index].isEditImage = true
                    this.bills[index].displayBtnChangeBillImage = "ใช้รูปใบเสร็จเดิม"
                } else {
                    this.displayBills[index].isHideFormInput = true
                    this.bills[index].isEditImage = false
                    this.bills[index].displayBtnChangeBillImage = "แก้ไขรูปใบเสร็จ"
                }

            },
            onAddIdCardFile: function (error, file) {
                this.idCardFileDisplay.file = file
                this.idCardFileDisplay.filename = file.filename
                this.idCardFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            onAddDiaryFile: function (error, file) {
                this.diaryFileDisplay.file = file
                this.diaryFileDisplay.filename = file.filename
                this.diaryFileDisplay.base64 = file.getFileEncodeDataURL()
            },
            loadInvoicehd() {
                for (let i = 0; i < this.invoicehd.length; i++) {

                    for (let j = 0; j < this.hospitals.length; j++) {
                        if (this.hospitals[j].HOSPITALID == this.invoicehd[i].hosid) {
                            this.bills[i].selectHospital = this.hospitals[j].HOSPITALNAME
                            this.bills[i].selectHospitalId = this.hospitals[j].HOSPITALID;
                        }
                    }
                    for (let j = 0; j < this.wounded.length; j++) {
                        if (this.wounded[j].woundedId == parseInt(this.invoicehd[i].mainconsider)) {
                            console.log("mainconsi ", parseInt(this.invoicehd[i].mainconsider))
                            this.bills[i].injuri = this.wounded[j].wounded
                            this.bills[i].injuriId = this.wounded[j].woundedId;
                        }
                    }

                    console.log(this.bills[i].billFileShow)
                    this.bills[i].billNo = this.invoicehd[i].idInvhd;
                    this.bills[i].money = this.invoicehd[i].suminv;
                    this.bills[i].bookNo = this.invoicehd[i].bookNo;
                    this.bills[i].bill_no = this.invoicehd[i].receiptNo;
                    this.bills[i].typePatient = this.invoicehd[i].victimType
                    this.bills[i].hospitalized_date = moment(this.invoicehd[i].takendate).format("YYYY-MM-DD");
                    this.bills[i].hospitalized_time = this.invoicehd[i].takentime.replace('.', ':');
                    this.bills[i].out_hospital_date = moment(this.invoicehd[i].dispensedate).format("YYYY-MM-DD");
                    this.bills[i].out_hospital_time = this.invoicehd[i].dispensetime.replace('.', ':');
                    this.getBillFileFromECM(this.invoicehd[i].idInvhd);

                    if (i > 0) {
                        this.displayBills.push({
                            isDisabledInputImageBill: true, isDisabledImageBill: true, isDisabledConsider: true, isDisabledVictimType: true, isDisabledHospital: true,
                            isDisabledBookNo: true, isDisabledReceiptNo: true, isDisabledMoney: true, isDisabledTakenDate: true, isDisabledTakenTime: true, isDisabledDispenseDate: true, isDisabledDispenseTime: true
                        });
                    }
                    this.invNotPassDescList[i] = this.invoicehd[i].invNotPassTypeId.split("-");
                    for (let j = 0; j < this.invNotPassDescList[i].length; j++) {
                        if (this.invNotPassDescList[i][j] == '101') {
                            this.invNotPassDescList[i][j] = 'ภาพถ่ายไม่ใช่ใบเสร็จค่ารักษา'
                        } else if (this.invNotPassDescList[i][j] == '102') {
                            this.invNotPassDescList[i][j] = 'ภาพถ่ายใบเสร็จไม่ชัด'
                        } else if (this.invNotPassDescList[i][j] == '103') {
                            this.invNotPassDescList[i][j] = 'ข้อมูลประเภทผู้ป่วยไม่ตรงกับภาพถ่ายใบเสร็จ'
                        } else if (this.invNotPassDescList[i][j] == '104') {
                            this.invNotPassDescList[i][j] = 'ข้อมูลโรงพยาบาลไม่ตรงกับภาพถ่ายใบเสร็จ'
                        } else if (this.invNotPassDescList[i][j] == '105') {
                            this.invNotPassDescList[i][j] = 'ข้อมูลเลขที่ใบเสร็จไม่ตรงกับภาพถ่ายไม่เสร็จ'
                        } else if (this.invNotPassDescList[i][j] == '106') {
                            this.invNotPassDescList[i][j] = 'ข้อมูลเล่มที่ใบเสร็จไม่ตรงกับภาพถ่ายใบเสร็จ'
                        } else if (this.invNotPassDescList[i][j] == '107') {
                            this.invNotPassDescList[i][j] = 'ข้อมูลจำนวนเงินไม่ตรงกับภาพถ่ายใบเสร็จ'
                        }
                    }

                    var invNotPassTypeId = this.invoicehd[i].invNotPassTypeId.split("-");
                    for (let j = 0; j < invNotPassTypeId.length; j++) {
                        if (invNotPassTypeId[j] == '101' || invNotPassTypeId[j] == '102') {
                            this.displayBills[i].isDisabledInputImageBill = false
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledConsider = false
                            this.displayBills[i].isDisabledVictimType = false
                            this.displayBills[i].isDisabledHospital = false
                            this.displayBills[i].isDisabledBookNo = false
                            this.displayBills[i].isDisabledReceiptNo = false
                            this.displayBills[i].isDisabledMoney = false
                            this.displayBills[i].isDisabledTakenDate = false
                            this.displayBills[i].isDisabledTakenTime = false
                            this.displayBills[i].isDisabledDispenseDate = false
                            this.displayBills[i].isDisabledDispenseTime = false
                            this.displayBills[i].isHideFormInput = true
                        } else if (invNotPassTypeId[j] == '103') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledVictimType = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '104') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledHospital = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '105') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledReceiptNo = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '106') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledBookNo = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '107') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledMoney = false
                            this.displayBills[i].isHideFormInput = false
                        }

                    }
                    console.log("sss: ", this.displayBills)
                    console.log("bbb: ", this.invNotPassDescList)

                }
            },
            getInvoicehd() {
                console.log('getInvoicehd');
                var url = this.$store.state.envUrl + '/api/approval/Invoicehd/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
                axios.get(url)
                    .then((response) => {
                        this.invoicehd = response.data;
                        console.log("invoicehd: ", response.data);
                        if (response.data.length > 1) {
                            this.addField(null, this.bills);
                        }
                        this.loadInvoicehd();
                        this.calMoney();
                        this.getChangwatNames();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getDocumentReceive() {
                console.log('getDocumentReceive');
                var url = this.$store.state.envUrl + '/api/Approval/DocumentReceive/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
                axios.get(url)
                    .then((response) => {
                        this.getBankFileFromECM()
                        if (response.data != null) {
                            for (let i = 0; i < this.bankNames.length; i++) {
                                if (response.data.accountBankName == this.bankNames[i].bankCode) {
                                    this.inputBank.accountBankName = this.bankNames[i].name
                                    this.inputBank.bankId = this.bankNames[i].bankCode
                                    this.inputBank.accountName = response.data.accountName
                                    this.inputBank.accountNumber = response.data.accountNumber
                                    this.inputBank.isEditBankImage = false
                                    this.inputBank.displayBtnChangeBankImage = "แก้ไขรูปบัญชีรับเงิน"
                                    this.isLoading = false;
                                    return true;
                                }
                            }

                        }
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getBankNames() {
                console.log('getBankNames');
                var url = this.$store.state.envUrl + '/api/Master/Bank';
                axios.get(url)
                    .then((response) => {
                        this.bankNames = response.data;
                        console.log(response.data);
                        this.getDocumentReceive();
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getHospitalNames() {
                console.log('getHospitalNames');
                var url = "https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/api/Utility/Hospital";
                axios.post(url)
                    .then((response) => {
                        this.hospitals = response.data.data;
                        console.log(this.hospitals);
                        this.getWoundeds();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getChangwatNames() {
                console.log('getChangwatNames');
                var url = this.$store.state.envUrl + '/api/Master/Changwat';
                axios.get(url)
                    .then((response) => {
                        this.changwats = response.data;
                        console.log(response.data);
                        this.getBankNames();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getWoundeds() {
                console.log('getWoundeds');
                var url = this.$store.state.envUrl + '/api/Master/Wounded';
                axios.get(url)
                    .then((response) => {
                        this.wounded = response.data.woundedList;
                        this.organ = response.data.organ
                        console.log(response.data);

                        this.getInvoicehd();

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getDocumentCheck() {
                console.log('getDocumentCheck');
                var url = this.$store.state.envUrl + '/api/Approval/DocumentCheck/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
                axios.get(url)
                    .then((response) => {
                        this.documentCheck = response.data;
                        console.log("docchk: ", this.documentCheck)
                        if (this.documentCheck != null) {
                            if (this.documentCheck.bookbankStatus == "N") {
                                this.accountDoc = true;
                                this.bookbankNotPassDescList = this.documentCheck.bbCommentTypeId.split("-");
                                for (let i = 0; i < this.bookbankNotPassDescList.length; i++) {
                                    if (this.bookbankNotPassDescList[i] == "201") {
                                        this.bookbankNotPassDescList[i] = 'ภาพถ่ายไม่ใช่บัญชีธนาคาร'
                                    } else if (this.bookbankNotPassDescList[i] == "202") {
                                        this.bookbankNotPassDescList[i] = 'ภาพถ่ายบัญชีธนาคารไม่ชัด'
                                    } else if (this.bookbankNotPassDescList[i] == "203") {
                                        this.bookbankNotPassDescList[i] = 'ข้อมูลชื่อธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
                                    } else if (this.bookbankNotPassDescList[i] == "204") {
                                        this.bookbankNotPassDescList[i] = 'ข้อมูลชื่อบัญชีไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
                                    } else if (this.bookbankNotPassDescList[i] == "205") {
                                        this.bookbankNotPassDescList[i] = 'ข้อมูลเลขที่บัญชีธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
                                    }
                                }

                                var bookbankNotPassTypeId = this.documentCheck.bbCommentTypeId.split("-");
                                for (let i = 0; i < bookbankNotPassTypeId.length; i++) {
                                    if (bookbankNotPassTypeId[i] == "201" || bookbankNotPassTypeId[i] == "202") {
                                        this.displayBankAccount.isDisabledInputImageBookBank = false
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledBankName = false
                                        this.displayBankAccount.isDisabledAccountName = false
                                        this.displayBankAccount.isDisabledAccountNumber = false
                                    } else if (bookbankNotPassTypeId[i] == "203") {
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledBankName = false
                                    } else if (bookbankNotPassTypeId[i] == "204") {
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledAccountName = false
                                    } else if (bookbankNotPassTypeId[i] == "205") {
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledAccountNumber = false
                                    }
                                }
                            }
                            if (this.documentCheck.invoiceStatus == "N") {
                                this.invoiceDoc = true;
                            }
                        }

                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            getBillFileFromECM(idInvhd) {
                var url = this.$store.state.envUrl + '/api/Approval/DownloadFromECM'
                const body = {
                    SystemId: '02',
                    TemplateId: '03',
                    DocumentId: '01',
                    RefId: idInvhd + '|' + this.accData.accNo + '|' + this.accData.victimNo,
                };
                axios.post(url, JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then((response) => {
                    for (let i = 0; i < this.invoicehd.length; i++) {
                        if (this.invoicehd[i].idInvhd == idInvhd) {
                            this.bills[i].billFileShow = 'data:image/png;base64,' + response.data
                        }
                    }
                }).catch(function (error) {
                    console.log(error);
                });

            },
            getBankFileFromECM() {
                var url = this.$store.state.envUrl + '/api/Approval/DownloadFromECM'
                const body = {
                    SystemId: '03',
                    TemplateId: '09',
                    DocumentId: '01',
                    RefId: this.$route.params.appNo + '|' + this.accData.accNo + '|' + this.accData.victimNo,
                };
                axios.post(url, JSON.stringify(body), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then((response) => {
                    this.bankFileDisplay.base64 = 'data:image/png;base64,' + response.data;
                }).catch(function (error) {
                    console.log(error);
                });

            },
            onAddBankAccountFile: function (error, file) {
                this.bankFileDisplay.file = file
                this.inputBank.bankBase64String = file.getFileEncodeBase64String()
                this.inputBank.bankFilename = file.filename

            },
            onAddBillFile: function (index) {
                this.bills[index].filename = this.bills[index].file[0].filename
                this.bills[index].editBillImage = this.bills[index].file[0].getFileEncodeBase64String()
                console.log("add bill: ", this.bills[index])
            },
            //getIt: function () {
            //    console.log(this.$refs.pond.getFiles());
            //},
            onChangwatChange() {

                this.divHospitalModal = true;

            },
            submitModalHospital(index) {
                this.bills[index].selectHospital = this.mockHospital
                this.modalHospital = false
                this.selectChangwat = 0;
                this.mockHospital = 0;
                this.divHospitalModal = false;
            },
            submitModalWounded(index) {
                this.bills[index].injuri = this.mockWounded
                this.modalWounded = false
                this.selectOrgan = 0;
                this.mockWounded = 0;
                this.divWoundedModal = false;
            },
            submit() {
                this.submitted = true;

                // stop here if form is invalid
                this.$v.$touch();
                if (this.$v.$invalid) {
                    return false;
                }
                console.log("ss   ", JSON.stringify(this.bills))
                for (let i = 0; i < this.bills.length; i++) {
                    this.bills[i].accNo = this.hosAppData.accNo
                    this.bills[i].victimNo = this.accData.victimNo
                    this.bills[i].reqNo = this.$route.params.appNo
                    this.bills[i].money = this.bills[i].money.toString()
                }
                var url = this.$store.state.envUrl + '/api/Approval/CheckInvoiceUsing'
                let isDuplicate = false;
                this.isLoading = true;
                axios.post(url, JSON.stringify(this.bills), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then((response) => {
                    var billIdDuplicate = [];
                    this.duplicateInv = response.data
                    console.log(this.duplicateInv)
                    if (this.duplicateInv.length > 0) {
                        for (let i = 0; i < this.duplicateInv.length; i++) {
                            if (this.duplicateInv[i].isDuplicate == true) {
                                isDuplicate = true;
                                billIdDuplicate.push(i + 1);
                            }
                        }
                    }
                    if (billIdDuplicate.length > 0) {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'info',
                            html: '<p align="left"> <strong>ใบเสร็จที่ : ' + billIdDuplicate + ' </strong><br>&emsp;&emsp;เคยใช้ในการเบิกค่าเสียหายเบื้องต้นไปแล้ว กรุณาใช้ใบเสร็จค่ารักษาอื่น </p>',
                            title: 'แจ้งเตือน',
                            /*footer: '<a href="">Why do I have this issue?</a>'*/
                            showCancelButton: false,
                            showDenyButton: false,

                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {

                            }
                        })
                        return false;
                    } else {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'info',
                            text: 'ท่านยืนยันที่จะส่งเอกสารเพิ่มเติมหรือไม่?',
                            title: 'แจ้งเตือน',
                            /*footer: '<a href="">Why do I have this issue?</a>'*/
                            showCancelButton: false,
                            showDenyButton: true,
                            denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยกเลิก",
                            denyButtonColor: '#dad5e9',
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยืนยัน",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {

                            }
                        }).then((result) => {
                            this.$store.state.inputApprovalData.AccNo = this.accData.accNo
                            this.$store.state.inputApprovalData.VictimNo = this.accData.victimNo
                            this.$store.state.inputApprovalData.AppNo = this.$route.params.appNo
                            this.$store.state.inputApprovalData.SumMoney = this.total_amount
                            this.$store.state.inputApprovalData.ClaimNo = this.accData.lastClaim.claimNo
                            this.$store.state.inputApprovalData.UserIdLine = this.$store.state.userTokenLine
                            this.$store.state.inputApprovalData.Injury = this.injuri
                            this.$store.state.inputApprovalData.BillsData = this.bills
                            this.$store.state.inputApprovalData.BankData = this.inputBank
                            this.$store.state.inputApprovalData.VictimData = null
                            console.log(this.$store.state.inputApprovalData)
                            console.log(result)
                            if (result.isConfirmed) {
                                this.$router.push({ name: 'ConfirmOTP', params: { from: "AddDocument" } })
                            }
                            //} else if (result.isDenied) {

                            //}
                        });
                    }


                }).catch(function (error) {
                    alert(error);
                });
                if (isDuplicate) {
                    return false;
                } else {

                    return true;
                }



            },
            calMoney() {
                let sum = 0;
                for (let i = 0; i < this.bills.length; i++) {

                    sum = sum + parseInt(this.bills[i].money)
                }
                this.total_amount = sum

            },
            addField(value, fieldType) {
                var index = this.bills.length + 1
                fieldType.push({
                    billNo: index, bill_no: "", bookNo: "", selectHospital: '', money: "", hospitalized_date: "", hospitalized_time: "",
                    out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: "",
                    file: null, billFileShow: "", filename: "", editBillImage: "", isEditImage: false, displayBtnChangeBillImage: "แก้ไขรูปใบเสร็จ"
                });
                this.calMoney()
                console.log("fieldType: ", fieldType)
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
                this.calMoney()
                console.log(this.bills)
            },

            showSwal() {
                this.$swal({
                    icon: 'success',
                    //text: 'ท่านสามารถติดตามผลดำเนินการได้ที่เมนูติดตามสถานะ',
                    title: 'ส่งเอกสารเพิ่มเติมแล้ว',
                    showCancelButton: false,
                    showDenyButton: false,
                    confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    confirmButtonColor: '#5c2e91',
                    willClose: () => {
                        this.$router.push({ name: 'CheckStatus', params: { id: this.accData.stringAccNo } })
                    }
                })
            },
            showBigImage(src) {
                this.modalBigImage = true
                this.srcBigImage = src
            },
            calIndex(index) {

                return parseInt(index) + 1;
            }
        },
        async mounted() {
            await this.getHospitalNames();
            await this.getDocumentCheck();



            this.selectChangwat = 0;
            this.selectHospital = 0;
            console.log('accData', this.accData);
            console.log('hosData', this.hosAppData);
        },

        computed: {
            filteredItems: function () {
                return this.hospitals.filter(function (el) {
                    return el.CHANGWATSHORTNAME === this.selectChangwat;
                }, this);
            },
            filteredWoundedItems: function () {
                return this.wounded.filter(function (el) {
                    return el.organ === this.selectOrgan;
                }, this);
            }
        }
    };
</script>

<style>
    .big-img-show {
        margin: 5px;
        width: 95%;
        border-radius: 10px;
    }
</style>