<template>
    <div class="container" align="center">
        <loading :active.sync="isLoading"
                 :can-cancel="false"
                 color="#5c2e91"
                 loader="dots"
                 :is-full-page="true">

        </loading>
        <h2 id="header2" class="mb-4">แก้ไขข้อมูล/แนบเอกสารเพิ่มเติม</h2>
        <div align="left">
            <p style="font-weight: bold">
                <ion-icon name="newspaper-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> เลขที่รับแจ้ง:
                <span class="p-main-color">{{ iclaimAppData.accNo }}</span> คำร้องที่:
                <span class="p-main-color">{{ iclaimAppData.appNo }}</span>
                <br>
                <ion-icon name="calendar-outline" style="font-size: 20px; margin-bottom: -5px;"></ion-icon> วันที่ยื่นคำร้อง:
                <span class="p-main-color">{{ iclaimAppData.stringRegDate }}</span>
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
            <p class="p-main-color">จำนวนเงิน {{iclaimAppData.sumMoney}} บาท</p>
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
                    <div style="color: #2C3E50; padding-left: 5px"  v-if="documentCheck.bookbankComment">
                        *หมายเหตุเพิ่มเติม : {{documentCheck.bookbankComment}}
                    </div>
                </div>
            </div>
            <div v-if="invNotPassDescList.length > 0" style="padding-left: 10px; margin-top: 10px;">
                <div style="color: var(--main-color)" v-for="(input, indexi) in invNotPassDescList" :key="indexi">
                    ใบเสร็จที่ : {{indexi + 1}}
                    <div style="color: red; padding-left: 5px" v-for="(value, indexj) in invNotPassDescList[indexi]" :key="indexj">
                        - {{invNotPassDescList[indexi][indexj]}}
                    </div>
                    <div style="color: #2C3E50; padding-left: 5px" v-if="invoicehd[indexi].invNotPassComment">
                        *หมายเหตุเพิ่มเติม : {{invoicehd[indexi].invNotPassComment}}
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
                                   ref="pondBankEdit"
                                   credits="null"
                                   label-idle="กดที่นี่เพื่ออัพโหลดรูปบัญชีธนาคาร1"
                                   v-bind:allow-multiple="false"
                                   accepted-file-types="image/jpeg, image/png"
                                   v-on:addfile="onAddBankAccountFile"
                                   v-if="inputBank.isEditBankImage"
                                   allowFileSizeValidation="true"
                                   maxFileSize="5MB"
                                   labelMaxFileSizeExceeded="รูปมีขนาดใหญ่เกินไป"
                                   labelMaxFileSize="ขนาดของรูปภาพต้องไม่เกิน {filesize}" />
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
                <div v-if="!displayBankAccount.isHideFormInput ">
                    <p class="mb-0 px-2">
                        ชื่อธนาคาร
                        <span class="star-require">*</span>
                    </p>
                    <div class="mb-2">
                        <select :disabled="displayBankAccount.isDisabledBankName" v-model="$v.inputBank.accountBankName.$model" style="font-size: 13px;" :class="{ 'is-invalid': $v.inputBank.accountBankName.$error }">
                            <option v-for="bankName in bankNames" :value="bankName.name" v-bind:key="bankName.bank" style="font-size: 12px; line-height: 0px">
                                {{ bankName.name }}
                            </option>
                        </select>
                        <div v-if="submitted && !$v.inputBank.accountBankName.required" class="invalid-feedback" style="margin-top:-5px;">กรุณาเลือกบัญชีธนาคาร</div>
                    </div>
                    <p class="mb-0 px-2">
                        ชื่อบัญชีธนาคาร
                        <span class="star-require">*</span>
                    </p>
                    <b-form-input :disabled="displayBankAccount.isDisabledAccountName" class="mt-0 mb-2" v-model="$v.inputBank.accountName.$model" :class="{ 'is-invalid': $v.inputBank.accountName.$error }"></b-form-input>
                    <div v-if="submitted && !$v.inputBank.accountName.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกชื่อบัญชีธนาคาร</div>
                    <p class="mb-0 px-2">
                        เลขบัญชีธนาคาร
                        <span class="star-require">*</span>
                    </p>
                    <b-form-input :disabled="displayBankAccount.isDisabledAccountNumber" class="mt-0 mb-2" v-model="$v.inputBank.accountNumber.$model" type="tel" v-mask="'###-#-#####-###'" :maxlength="15" :class="{ 'is-invalid': $v.inputBank.accountNumber.$error }"></b-form-input>
                    <div v-if="submitted && !$v.inputBank.accountNumber.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขที่บัญชี</div>
                </div>

                <br>
            </div>
        </div>
        <div v-if="invoiceDoc">

            <br />
            <div v-for="(input, index) in $v.bills.$each.$iter" :key="index">
                <b-overlay :show="invOverlay[index].isShow" :id="'billOverlay' + index">
                    <div class="box-container mt-2">
                        <div class="input wrapper flex items-center">
                            <div align="left">
                                <label>ใบเสร็จรับเงินค่ารักษาพยาบาลที่ : {{calIndex(index)}}</label>
                            </div>
                            <div v-if="!displayBills[index].isDisabledInputImageBill">
                                <file-pond credits="null"
                                           ref="pondBillEdit"
                                           label-idle="กดที่นี่เพื่ออัพโหลดใบเสร็จค่ารักษา"
                                           v-bind:allow-multiple="false"
                                           v-bind:allowFileEncode="true"
                                           accepted-file-types="image/jpeg, image/png"
                                           v-if="bills[index].isEditImage"
                                           v-bind:files="bills[index].file"
                                           v-model="bills[index].file"
                                           v-on:addfile="onAddBillFile(index)"
                                           v-on:removefile="onRemoveBillFile(index)"
                                           v-on:error="onError"
                                           allowFileSizeValidation="true"
                                           maxFileSize="7MB"
                                           labelMaxFileSizeExceeded="รูปมีขนาดใหญ่เกินไป"
                                           labelMaxFileSize="ขนาดของรูปภาพต้องไม่เกิน {filesize}" />
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
                                <label class="px-2">อาการบาดเจ็บ<span class="star-require">*</span></label>
                                <b-form-input :disabled="displayBills[index].isDisabledConsider" class="mt-0 mb-2" v-model="input.injuri.$model" placeholder="" type="text" @click="modalWounded=!modalWounded" :class="{ 'is-invalid': input.injuri.$error }"></b-form-input>
                                <div v-if="submitted && !input.injuri.required" class="invalid-feedback" style="margin-top: -5px;">กรุณาเลือกอาการบาดเจ็บ</div>
                                <label class="px-2">ประเภทผู้ป่วย<span class="star-require">*</span></label>
                                <br />
                                <div class="mt-1 mb-2" style="float: left;">
                                    <vs-radio :disabled="displayBills[index].isDisabledVictimType" v-model="input.typePatient.$model" val="IPD" style="float: left">
                                        ผู้ป่วยใน (นอนพักรักษาตัวในโรงพยาบาลตั้งแต่ 6 ชั่วโมงขึ้นไป)
                                    </vs-radio>
                                    <vs-radio :disabled="displayBills[index].isDisabledVictimType" v-model="input.typePatient.$model" val="OPD" class="mt-1" style="float: left">
                                        ผู้ป่วยนอก (ไม่ได้นอนพักรักษาตัวในโรงพยาบาล)
                                    </vs-radio>

                                </div>
                                <label class="px-2">โรงพยาบาล<span class="star-require">*</span></label>
                                <b-form-input :disabled="displayBills[index].isDisabledHospital" class="mt-0 mb-2" v-model="input.selectHospital.$model" type="text" @click="modalHospital=!modalHospital" :class="{ 'is-invalid': input.selectHospital.$error }" />
                                <div v-if="submitted && !input.selectHospital.required" class="invalid-feedback" style="margin-top: -5px;">กรุณาเลือกโรงพยาบาล</div>
                                <div class="row">
                                    <div class="col-6">
                                        <label class="px-2">ใบเสร็จเล่มที่<span class="star-require">*</span></label>
                                        <b-form-input :disabled="displayBills[index].isDisabledBookNo" class="mt-0 mb-2" v-model="input.bookNo.$model" type="number" :maxlength="10" placeholder="" :class="{ 'is-invalid': input.bookNo.$error }" />
                                        <div v-if="submitted && !input.bookNo.required" class="invalid-feedback" style="margin-top: -5px;">กรุณากรอกเล่มที่ใบเสร็จ</div>
                                    </div>
                                    <div class="col-6">
                                        <label class="px-2">เลขที่ใบเสร็จ<span class="star-require">*</span></label>
                                        <b-form-input :disabled="displayBills[index].isDisabledReceiptNo" class="mt-0 mb-2" v-model="input.bill_no.$model" type="number" :maxlength="10" placeholder="" :class="{ 'is-invalid': input.bill_no.$error }" />
                                        <div v-if="submitted && !input.bill_no.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกเลขที่ใบเสร็จ</div>
                                    </div>
                                </div>
                                <label class="px-2">จำนวนเงิน<span class="star-require">*</span></label>
                                <b-form-input :disabled="displayBills[index].isDisabledMoney" class="mt-0 mb-2" v-model="input.money.$model" type="number" placeholder="" @change="rmLeadingZero(index)"  v-mask="'####'"  :class="{ 'is-invalid': input.money.$error }" />
                                <div v-if="submitted && !input.money.required" class="invalid-feedback" style="margin-top:-5px;">กรุณากรอกจำนวนเงิน</div>
                                <div class="row">
                                    <div class="col-8">
                                        <label class="px-2">วันที่เข้ารักษา<span class="star-require">*</span></label>
                                    </div>
                                    <div class="col-4">
                                        <label class="px-2">เวลา<span class="star-require">*</span></label>
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
                            <div class="text-center" v-if="!displayBills[index].isHideBtnCansel">
                                <button class="btn-confirm-money" type="button" @click="onBtnCanselInvClick(index)">ยกเลิกใบเสร็จนี้</button>
                            </div>
                            <br>
                        </div>
                    </div>
                    <template #overlay>
                        <div class="text-center">
                            <p id="cancel-label">ใบเสร็จนี้ถูกยกเลิกจากการเบิกครั้งนี้แล้ว</p>
                            <button class="btn-confirm-money" type="button" @click="onBtnCanselInvClick(index)">X</button>
                        </div>
                    </template>
                </b-overlay>

            </div>
            <br>
            <p class="p_right">รวมจำนวนเงินที่ขอเบิก: {{ total_amount }} บาท</p>
            <br>
        </div>
        <br />
        <br />
        <button class="btn-confirm-money" type="button" @click="submit">ยืนยัน/ส่งเอกสารเพิ่มเติม</button>
        <br>
        <br>
        <br>
    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'
    import axios from 'axios'
    import * as moment from "moment/moment";

    import 'vue-form-wizard/dist/vue-form-wizard.min.css'
    import vueFilePond from 'vue-filepond';
    import 'filepond/dist/filepond.min.css'// Import FilePond styles
    // Import FilePond plugins
    // Please note that you need to install these plugins separately
    // Import image preview plugin styles
    import 'filepond-plugin-image-preview/dist/filepond-plugin-image-preview.min.css';
    //Your Javascript lives within the Script Tag
    import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
    import FilePondPluginImagePreview from 'filepond-plugin-image-preview';
    import FilePondPluginFileEncode from 'filepond-plugin-file-encode';
    import FilePondPluginFileValidateSize from 'filepond-plugin-file-validate-size';

    const FilePond = vueFilePond(FilePondPluginFileValidateType, FilePondPluginImagePreview, FilePondPluginFileEncode, FilePondPluginFileValidateSize);

    // Import loading-overlay
    import Loading from 'vue-loading-overlay';

    import { required } from "vuelidate/lib/validators";

    export default {
        name: "AddDocument",
        mixins: [mixin],
        components: {
            Loading,
            FilePond
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
                    billNo: 1, bill_no: "", bookNo: "", selectHospital: '', money: "0", hospitalized_date: "", hospitalized_time: ""
                    , out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: ""
                    , file: null, billFileShow: "", filename: "", editBillImage: "", isEditImage: false, displayBtnChangeBillImage: "แก้ไขรูปใบเสร็จ"
                    , isCancel: false
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
                iclaimAppData: this.$store.getters.hosAppGetter(this.$route.params.id, this.$route.params.appNo),
                // Document Checkbox
                invoiceDoc: false,
                doc2: false,
                doc3: false,
                accountDoc: false,
                isLoading: true,
                displayBankAccount: { isDisabledInputImageBookBank: true, isDisabledImageBookBank: true, isDisabledBankName: true, isDisabledAccountName: true, isDisabledAccountNumber: true, isHideFormInput: true },
                displayBills: [{
                    isDisabledInputImageBill: true, isDisabledImageBill: true, isDisabledConsider: true, isDisabledVictimType: true, isDisabledHospital: true,
                    isDisabledBookNo: true, isDisabledReceiptNo: true, isDisabledMoney: true, isDisabledTakenDate: true, isDisabledTakenTime: true,
                    isDisabledDispenseDate: true, isDisabledDispenseTime: true, isHideFormInput: true, isHideBtnCansel: true
                }],
                submitted: false,
                modalBigImage: false,
                srcBigImage: null,
                invNotPassDescList: [],
                bookbankNotPassDescList: [],
                invOverlay: [{ isShow: false }],
                invNotPassTypesList: null,
                invMustCanselList: null,
                bankAccountNotPassTypesList: null,
                beforeInputBank: { accountName: null, accountNumber: null, accountBankName: null, bankId: null, bankBase64String: '', bankFilename: '', isEditBankImage: false }




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
                if (!this.inputBank.isEditBankImage) {
                    this.displayBankAccount.isHideFormInput = false
                    this.inputBank.isEditBankImage = true
                    this.inputBank.displayBtnChangeBankImage = "ใช้รูปบัญชีเดิม"
                } else {
                    this.displayBankAccount.isHideFormInput = true
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
            onBtnCanselInvClick(index) {
                if (this.bills[index].isCancel) {
                    this.bills[index].isCancel = false
                    this.invOverlay[index].isShow = false
                    this.displayBills[index].isHideBtnCansel = false
                    this.calMoney()

                } else {
                    this.bills[index].isCancel = true
                    this.invOverlay[index].isShow = true
                    this.displayBills[index].isHideBtnCansel = true
                    this.total_amount = this.total_amount - this.bills[index].money
                }

            },
            invNotPassMustCansel(listInvMustCansel) {
                if (listInvMustCansel.length > 0) {
                    this.isLoading = false;
                    var htmlMessage = '<p style="margin-bottom:10px" align="left"> <strong>ใบเสร็จที่ : ' + listInvMustCansel.map(m => m.bill_no) + ' </strong><br>&emsp;&emsp;ไม่ใช่ใบเสร็จค่ารักษาที่เกิดเหตุจากรถ </p>'
                    htmlMessage += '<p style="color:red;margin-bottom:5px" align="left">&emsp;&emsp;*หมายเหตุ หากท่านต้องการยกเลิกใบเสร็จในการเบิกคำร้องนี้ กรุณาติ๊กเลือกใบเสร็จแล้วกดปุ่ม <label style="color:var(--main-color)">"ยกเลิกใบเสร็จ"</label> </p>'
                    for (let i = 0; i < listInvMustCansel.length; i++) {
                        htmlMessage += '<div class="form-check px-5"><input id="swal-check' + (i + 1) + '" class="form-check-input" type="checkbox" value="" ><label class="form-check-label" for="swal-check' + (i + 1) + '">ยกเลิกใบเสร็จที่ : ' + listInvMustCansel[i].bill_no + '</label></div>'
                    }

                    this.$swal({
                        icon: 'warning',
                        html: htmlMessage,
                        title: 'คำเตือน',
                        showCancelButton: false,
                        showDenyButton: true,
                        denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        denyButtonColor: '#dad5e9',
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยกเลิกใบเสร็จ",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {

                        }
                    }).then((result) => {
                        if (result.isConfirmed) {
                            for (let i = 0; i < listInvMustCansel.length; i++) {
                                if (document.getElementById('swal-check' + (i + 1)).checked) {
                                    let index = this.bills.findIndex(w => w.billNo === listInvMustCansel[i].idInvhd)
                                    this.bills[index].isCancel = true
                                    this.invOverlay[index].isShow = true
                                    this.displayBills[index].isHideBtnCansel = true
                                    this.total_amount = this.total_amount - this.bills[index].money
                                }
                            }                           
                        }
                    });

                }
            },
            loadInvoicehd() {
                var billsMustCansel = []
                for (let i = 0; i < this.invoicehd.length; i++) {

                    for (let j = 0; j < this.hospitals.length; j++) {
                        if (this.hospitals[j].HOSPITALID == this.invoicehd[i].hosid) {
                            this.bills[i].selectHospital = this.hospitals[j].HOSPITALNAME
                            this.bills[i].selectHospitalId = this.hospitals[j].HOSPITALID;
                        }
                    }

                    for (let j = 0; j < this.wounded.length; j++) {
                        if (this.wounded[j].woundedId == parseInt(this.invoicehd[i].mainconsider)) {
                            this.bills[i].injuri = this.wounded[j].wounded
                            this.bills[i].injuriId = this.wounded[j].woundedId;
                        }
                    }
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
                        for (let l = 0; l < this.invNotPassTypesList.length; l++) {
                            if (this.invNotPassDescList[i][j] == this.invNotPassTypesList[l].typeId) {
                                this.invNotPassDescList[i][j] = this.invNotPassTypesList[l].typeName
                            }
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
                        } else if (invNotPassTypeId[j] == '108') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledTakenDate = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '109') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledTakenTime = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '110') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledDispenseDate = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '111') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isDisabledDispenseTime = false
                            this.displayBills[i].isHideFormInput = false
                        } else if (invNotPassTypeId[j] == '112') {
                            this.displayBills[i].isDisabledImageBill = false
                            this.displayBills[i].isHideFormInput = true
                            billsMustCansel.push({
                                idInvhd: this.invoicehd[i].idInvhd,
                                bill_no: (i + 1)
                            })
                        }

                    }


                }
                this.invMustCanselList = billsMustCansel;
                this.invNotPassMustCansel(billsMustCansel);
            },
            
            getHospitalNames() {
                var url = "https://ts2thairscapi.rvpeservice.com/3PAccidentAPI/api/Utility/Hospital";
                axios.post(url)
                    .then((response) => {
                        this.hospitals = response.data.data;
                        //this.getWoundeds();
                        this.initialDataEditApprovalPage(); //ทกสอบรวม api req
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            initialDataEditApprovalPage() {
                var url = this.$store.state.envUrl + '/api/approval/DataForEditApprovalPage/{accNo}/{victimNo}/{reqNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{reqNo}', this.$route.params.appNo);
                var apiConfig = {
                    headers: {
                        Authorization: "Bearer " + this.$store.state.jwtToken.token
                    }
                }
                axios.get(url, apiConfig)
                    .then((response) => {
                        this.wounded = response.data.woundeds.woundedList;
                        this.organ = response.data.woundeds.organ;
                        this.invoicehd = response.data.invoicesNotPass;
                        this.invNotPassTypesList = response.data.typesOfInvoiceNotPass
                        this.changwats = response.data.changwats;
                        this.bankNames = response.data.bankNames;
                        this.bankAccountNotPassTypesList = response.data.typesOfBankAccountNotPass;
                        this.documentCheck = response.data.accountChecks;
                        if (this.invoicehd.length > 0) {
                            for (let i = 0; i < (this.invoicehd.length - 1); i++) {
                                this.addField(null, this.bills);
                                this.invOverlay.push({
                                    isShow: false
                                });

                            }
                        }
                        this.loadInvoicehd();
                        this.calMoney();
                        this.getBankFileFromECM();
                        if (response.data.account != null) {
                            for (let i = 0; i < this.bankNames.length; i++) {
                                if (response.data.account.accountBankName == this.bankNames[i].bankCode) {                                   
                                    this.inputBank.accountBankName = this.bankNames[i].name
                                    this.inputBank.bankId = this.bankNames[i].bankCode
                                    this.inputBank.accountName = response.data.account.accountName
                                    this.inputBank.accountNumber = response.data.account.accountNumber
                                    this.inputBank.isEditBankImage = false
                                    this.inputBank.displayBtnChangeBankImage = "แก้ไขรูปบัญชีรับเงิน"
                                    this.isLoading = false;
                                    this.beforeInputBank.accountBankName = this.bankNames[i].name
                                    this.beforeInputBank.bankId = this.bankNames[i].bankCode
                                    this.beforeInputBank.accountName = response.data.account.accountName
                                    this.beforeInputBank.accountNumber = response.data.account.accountNumber
                                    /*return true;*/
                                }
                            }
                        }
                        
                        //from api documentCheck
                        if (this.documentCheck != null) {
                            if (this.documentCheck.bookbankStatus == "N") {
                                this.accountDoc = true;
                                this.bookbankNotPassDescList = this.documentCheck.bbCommentTypeId.split("-");
                                for (let i = 0; i < this.bookbankNotPassDescList.length; i++) {
                                    for (let j = 0; j < this.bankAccountNotPassTypesList.length; j++) {
                                        if (this.bookbankNotPassDescList[i] == this.bankAccountNotPassTypesList[j].typeId) {
                                            this.bookbankNotPassDescList[i] = this.bankAccountNotPassTypesList[j].typeName
                                        }
                                    }
                                    //if (this.bookbankNotPassDescList[i] == "201") {
                                    //    this.bookbankNotPassDescList[i] = 'ภาพถ่ายไม่ใช่บัญชีธนาคาร'
                                    //} else if (this.bookbankNotPassDescList[i] == "202") {
                                    //    this.bookbankNotPassDescList[i] = 'ภาพถ่ายบัญชีธนาคารไม่ชัด'
                                    //} else if (this.bookbankNotPassDescList[i] == "203") {
                                    //    this.bookbankNotPassDescList[i] = 'ข้อมูลชื่อธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
                                    //} else if (this.bookbankNotPassDescList[i] == "204") {
                                    //    this.bookbankNotPassDescList[i] = 'ข้อมูลชื่อบัญชีไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
                                    //} else if (this.bookbankNotPassDescList[i] == "205") {
                                    //    this.bookbankNotPassDescList[i] = 'ข้อมูลเลขที่บัญชีธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
                                    //}
                                }

                                var bookbankNotPassTypeId = this.documentCheck.bbCommentTypeId.split("-");
                                for (let i = 0; i < bookbankNotPassTypeId.length; i++) {
                                    if (bookbankNotPassTypeId[i] == "201" || bookbankNotPassTypeId[i] == "202") {
                                        this.displayBankAccount.isDisabledInputImageBookBank = false
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledBankName = false
                                        this.displayBankAccount.isDisabledAccountName = false
                                        this.displayBankAccount.isDisabledAccountNumber = false
                                        this.displayBankAccount.isHideFormInput = true
                                    } else if (bookbankNotPassTypeId[i] == "203") {
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledBankName = false
                                        this.displayBankAccount.isHideFormInput = false
                                    } else if (bookbankNotPassTypeId[i] == "204") {
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledAccountName = false
                                        this.displayBankAccount.isHideFormInput = false
                                    } else if (bookbankNotPassTypeId[i] == "205") {
                                        this.displayBankAccount.isDisabledImageBookBank = false
                                        this.displayBankAccount.isDisabledAccountNumber = false
                                        this.displayBankAccount.isHideFormInput = false
                                    }
                                }
                            }
                            if (this.documentCheck.invoiceStatus == "N") {
                                this.invoiceDoc = true;
                            }
                        }

                    })
                    .catch((error) => {
                        if (error.toString().includes("401")) {
                            this.getJwtToken()
                            this.initialDataEditApprovalPage()
                        }
                    });
            },
            //getInvoicehdNotPass() {
            //    var url = this.$store.state.envUrl + '/api/approval/InvoicehdNotPass/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
            //    var apiConfig = {
            //        headers: {
            //            Authorization: "Bearer " + this.$store.state.jwtToken.token
            //        }
            //    }
            //    axios.get(url, apiConfig)
            //        .then((response) => {
            //            this.invoicehd = response.data.invoicesNotPass;
            //            this.invNotPassTypesList = response.data.typesOfInvoiceNotPass
            //            console.log("TypesInv", this.invNotPassTypesList)
            //            if (this.invoicehd.length > 0) {
            //                for (let i = 0; i < (this.invoicehd.length - 1); i++) {
            //                    this.addField(null, this.bills);
            //                    this.invOverlay.push({
            //                        isShow: false
            //                    });

            //                }
            //            }
            //            this.loadInvoicehd();
            //            this.calMoney();
            //            this.getChangwatNames();

            //        })
            //        .catch((error) => {
            //            if (error.toString().includes("401")) {
            //                this.getJwtToken()
            //                this.getInvoicehdNotPass()
            //            }
            //        });
            //},
            //getDocumentReceive() {
            //    var url = this.$store.state.envUrl + '/api/Approval/DocumentReceive/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
            //    var apiConfig = {
            //        headers: {
            //            Authorization: "Bearer " + this.$store.state.jwtToken.token
            //        }
            //    }
            //    axios.get(url, apiConfig)
            //        .then((response) => {
            //            this.getBankFileFromECM()
            //            if (response.data != null) {
            //                for (let i = 0; i < this.bankNames.length; i++) {
            //                    if (response.data.accountBankName == this.bankNames[i].bankCode) {
            //                        this.inputBank.accountBankName = this.bankNames[i].name
            //                        this.inputBank.bankId = this.bankNames[i].bankCode
            //                        this.inputBank.accountName = response.data.accountName
            //                        this.inputBank.accountNumber = response.data.accountNumber
            //                        this.inputBank.isEditBankImage = false
            //                        this.inputBank.displayBtnChangeBankImage = "แก้ไขรูปบัญชีรับเงิน"
            //                        this.isLoading = false;
            //                        return true;
            //                    }
            //                }

            //            }
            //        })
            //        .catch(function (error) {
            //            alert(error);
            //        });
            //},
            //getBankNames() {
            //    var url = this.$store.state.envUrl + '/api/Master/Bank';
            //    axios.get(url)
            //        .then((response) => {
            //            this.bankNames = response.data;
            //            this.getDocumentReceive();
            //        })
            //        .catch(function (error) {
            //            alert(error);
            //        });
            //},
            //getChangwatNames() {
            //    var url = this.$store.state.envUrl + '/api/Master/Changwat';
            //    axios.get(url)
            //        .then((response) => {
            //            this.changwats = response.data;
            //            this.getBankNames();

            //        })
            //        .catch(function (error) {
            //            alert(error);
            //        });
            //},
            
            //getAccountAndMasterData() {
            //    var url = this.$store.state.envUrl + '/api/Approval/AccountAndMasterData';
            //    const body = {
            //        AccNo: this.accData.stringAccNo,
            //        VictimNo: parseInt(this.accData.victimNo),
            //        ReqNo: parseInt(this.$route.params.appNo),
            //    };
            //    var apiConfig = {
            //        headers: {
            //            'Authorization': "Bearer " + this.$store.state.jwtToken.token,
            //            'Content-Type': 'application/json',
            //        }
            //    }
            //    axios.post(url, JSON.stringify(body), apiConfig)
            //        .then((response) => {
            //            console.log("new ", response)
            //            this.getBankFileFromECM()
            //            var bankNames = response.data.bankNames;
            //            if (response.data.account != null) {
            //                for (let i = 0; i < this.bankNames.length; i++) {
            //                    if (response.data.account.accountBankName == this.bankNames[i].bankCode) {
            //                        this.inputBank.accountBankName = this.bankNames[i].name
            //                        this.inputBank.bankId = this.bankNames[i].bankCode
            //                        this.inputBank.accountName = response.data.account.accountName
            //                        this.inputBank.accountNumber = response.data.account.accountNumber
            //                        this.inputBank.isEditBankImage = false
            //                        this.inputBank.displayBtnChangeBankImage = "แก้ไขรูปบัญชีรับเงิน"
            //                        this.isLoading = false;
            //                        console.log("good")
            //                        return true;
            //                    }
            //                }

            //            }
            //        })
            //        .catch(function (error) {
            //            alert(error);
            //        });
               
            //},
            //getWoundeds() {
            //    var url = this.$store.state.envUrl + '/api/Master/Wounded';
            //    axios.get(url)
            //        .then((response) => {
            //            this.wounded = response.data.woundedList;
            //            this.organ = response.data.organ
            //            this.getInvoicehdNotPass();

            //        })
            //        .catch(function (error) {
            //            alert(error);
            //        });
            //},
            //getDocumentCheck() {
            //    var url = this.$store.state.envUrl + '/api/Approval/DocumentCheck/{accNo}/{victimNo}/{appNo}'.replace('{accNo}', this.accData.stringAccNo).replace('{victimNo}', this.accData.victimNo).replace('{appNo}', this.$route.params.appNo);
            //    var apiConfig = {
            //        headers: {
            //            Authorization: "Bearer " + this.$store.state.jwtToken.token
            //        }
            //    }
            //    axios.get(url, apiConfig)
            //        .then((response) => {
            //            this.documentCheck = response.data;
            //            if (this.documentCheck != null) {
            //                if (this.documentCheck.bookbankStatus == "N") {
            //                    this.accountDoc = true;
            //                    this.bookbankNotPassDescList = this.documentCheck.bbCommentTypeId.split("-");
            //                    for (let i = 0; i < this.bookbankNotPassDescList.length; i++) {
            //                        if (this.bookbankNotPassDescList[i] == "201") {
            //                            this.bookbankNotPassDescList[i] = 'ภาพถ่ายไม่ใช่บัญชีธนาคาร'
            //                        } else if (this.bookbankNotPassDescList[i] == "202") {
            //                            this.bookbankNotPassDescList[i] = 'ภาพถ่ายบัญชีธนาคารไม่ชัด'
            //                        } else if (this.bookbankNotPassDescList[i] == "203") {
            //                            this.bookbankNotPassDescList[i] = 'ข้อมูลชื่อธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
            //                        } else if (this.bookbankNotPassDescList[i] == "204") {
            //                            this.bookbankNotPassDescList[i] = 'ข้อมูลชื่อบัญชีไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
            //                        } else if (this.bookbankNotPassDescList[i] == "205") {
            //                            this.bookbankNotPassDescList[i] = 'ข้อมูลเลขที่บัญชีธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร'
            //                        }
            //                    }

            //                    var bookbankNotPassTypeId = this.documentCheck.bbCommentTypeId.split("-");
            //                    for (let i = 0; i < bookbankNotPassTypeId.length; i++) {
            //                        if (bookbankNotPassTypeId[i] == "201" || bookbankNotPassTypeId[i] == "202") {
            //                            this.displayBankAccount.isDisabledInputImageBookBank = false
            //                            this.displayBankAccount.isDisabledImageBookBank = false
            //                            this.displayBankAccount.isDisabledBankName = false
            //                            this.displayBankAccount.isDisabledAccountName = false
            //                            this.displayBankAccount.isDisabledAccountNumber = false
            //                            this.displayBankAccount.isHideFormInput = true
            //                        } else if (bookbankNotPassTypeId[i] == "203") {
            //                            this.displayBankAccount.isDisabledImageBookBank = false
            //                            this.displayBankAccount.isDisabledBankName = false
            //                            this.displayBankAccount.isHideFormInput = false
            //                        } else if (bookbankNotPassTypeId[i] == "204") {
            //                            this.displayBankAccount.isDisabledImageBookBank = false
            //                            this.displayBankAccount.isDisabledAccountName = false
            //                            this.displayBankAccount.isHideFormInput = false
            //                        } else if (bookbankNotPassTypeId[i] == "205") {
            //                            this.displayBankAccount.isDisabledImageBookBank = false
            //                            this.displayBankAccount.isDisabledAccountNumber = false
            //                            this.displayBankAccount.isHideFormInput = false
            //                        }
            //                    }
            //                }
            //                if (this.documentCheck.invoiceStatus == "N") {
            //                    this.invoiceDoc = true;
            //                }
            //            }

            //        })
            //        .catch((error) => {
            //            if (error.toString().includes("401")) {
            //                this.getJwtToken()
            //                this.getDocumentCheck()
            //            }
            //        });
            //},
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
                        'Content-Type': 'application/json',
                        'Authorization': "Bearer " + this.$store.state.jwtToken.token
                    }
                }).then((response) => {
                    for (let i = 0; i < this.invoicehd.length; i++) {
                        if (this.invoicehd[i].idInvhd == idInvhd) {
                            this.bills[i].billFileShow = 'data:image/png;base64,' + response.data
                        }
                    }
                }).catch(function (error) {
                    alert(error);
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
                        'Content-Type': 'application/json',
                        'Authorization': "Bearer " + this.$store.state.jwtToken.token
                    }
                }).then((response) => {
                    this.bankFileDisplay.base64 = 'data:image/png;base64,' + response.data;
                }).catch(function (error) {
                    alert(error);
                });

            },
            onAddBankAccountFile: function (error, file) {
                this.isLoading = true;
                this.bankFileDisplay.file = file

                if (error != null) {
                    this.isLoading = false;
                    this.$swal({
                        icon: 'error',
                        text: error.sub,
                        title: error.main,
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {
                            this.$refs.pondBankEdit.removeFiles()
                            return;
                        }
                    })
                } else {
                    this.inputBank.bankFilename = file.filename
                    if (file) {// Resize Image
                        var fileDataUrl = file.getFileEncodeDataURL()
                        var imgRes = new Image();
                        imgRes.src = fileDataUrl
                        var img = document.createElement("img");
                        img.onload = () => {
                            var canvas = document.createElement("canvas");
                            var MAX_WIDTH = 720;
                            var MAX_HEIGHT = 720;
                            var width = img.width;
                            var height = img.height;

                            if (width > height) {
                                if (width > MAX_WIDTH) {
                                    height *= MAX_WIDTH / width;
                                    width = MAX_WIDTH;
                                }
                            } else {
                                if (height > MAX_HEIGHT) {
                                    width *= MAX_HEIGHT / height;
                                    height = MAX_HEIGHT;
                                }
                            }
                            canvas.width = width;
                            canvas.height = height;
                            var ctx = canvas.getContext("2d");
                            ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                            this.inputBank.bankBase64String = canvas.toDataURL(file.file.type);
                            this.isLoading = false;

                        }
                        img.src = fileDataUrl
                    }
                }


            },
            onError: function (error, file) {
                this.isLoading = true;
                if (error != null) {
                    this.isLoading = false;
                    this.$swal({
                        icon: 'error',
                        text: error.sub,
                        title: error.main,
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {
                            for (let i = 0; i < this.$refs.pondBillEdit.length; i++) {
                                this.$refs.pondBillEdit[i].removeFile(file.id)
                            }

                        }
                    })
                }
            },
            onRemoveBillFile: function (index) {
                this.bills[index].editBillImage = ""
            },
            onAddBillFile: function (index) {
                this.isLoading = true;
                if (this.bills[index].file[0].fileSize < 7000000) {
                    this.bills[index].filename = this.bills[index].file[0].filename
                    if (this.bills[index].file[0]) {// Resize Image
                        var fileDataUrl = this.bills[index].file[0].getFileEncodeDataURL()
                        var imgRes = new Image();
                        imgRes.src = fileDataUrl
                        var img = document.createElement("img");
                        img.onload = () => {
                            var canvas = document.createElement("canvas");
                            var MAX_WIDTH = 720;
                            var MAX_HEIGHT = 720;
                            var width = img.width;
                            var height = img.height;

                            if (width > height) {
                                if (width > MAX_WIDTH) {
                                    height *= MAX_WIDTH / width;
                                    width = MAX_WIDTH;
                                }
                            } else {
                                if (height > MAX_HEIGHT) {
                                    width *= MAX_HEIGHT / height;
                                    height = MAX_HEIGHT;
                                }
                            }
                            canvas.width = width;
                            canvas.height = height;
                            var ctx = canvas.getContext("2d");
                            ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                            this.bills[index].editBillImage = canvas.toDataURL(this.bills[index].file[0].file.type);
                            this.isLoading = false;

                        }
                        img.src = fileDataUrl
                    }
                }
            },
            onChangwatChange() {
                this.divHospitalModal = true;

            },
            submitModalHospital(index) {
                this.bills[index].selectHospital = this.mockHospital
                for (let i = 0; i < this.hospitals.length; i++) {
                    if (this.bills[index].selectHospital == this.hospitals[i].HOSPITALNAME) {
                        this.bills[index].selectHospitalId = this.hospitals[i].HOSPITALID
                    }
                }
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
            swalValidateAndCheckEdit(text) {
                this.$swal({
                    icon: 'warning',
                    html: 'หากท่านต้องการที่จะทำการเบิกในครั้งนี้ กรุณา' + text,
                    title: 'คำเตือน',
                    showCancelButton: false,
                    showConfirmButton: false,
                    showDenyButton: true,
                    denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                    denyButtonColor: '#dad5e9',

                    willClose: () => {

                    }
                })
                
            },
            submit() {
                this.submitted = true;
                // stop here if form is invalid
                this.$v.$touch();
                if (this.$v.$invalid) {
                    return false;
                }
                for (let i = 0; i < this.bankNames.length; i++) {
                    if (this.inputBank.accountBankName == this.bankNames[i].name) {
                        this.inputBank.bankId = this.bankNames[i].bankCode
                    }
                }
                //เช็คการแก้ไขตาม types ของใบเสร็จ
                var checkInvEditList = this.invNotPassDescList
                for (let i = 0; i < this.bills.length; i++) {
                    if (!this.bills[i].isCancel) {
                        for (let j = 0; j < this.invMustCanselList.length; j++) {
                            if (this.invMustCanselList[j].idInvhd == this.bills[i].billNo) {
                                this.swalValidateAndCheckEdit('ยกเลิกใบเสร็จค่ารักษาที่ไม่ได้เกิดเหตุจากรถ')
                                return false
                            }
                        }
                    }
                    if (checkInvEditList) {
                        for (let j = 0; j < checkInvEditList[i].length; j++) {
                            for (let l = 0; l < this.invNotPassTypesList.length; l++) {
                                if (checkInvEditList[i][j] == this.invNotPassTypesList[l].typeName && checkInvEditList[i][j] != 'ไม่ใช่ใบเสร็จค่ารักษาที่เกิดเหตุจากรถ') {
                                    if (this.bills[i].money == this.invoicehd[i].suminv && checkInvEditList[i][j] == 'ข้อมูลจำนวนเงินไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].selectHospitalId == this.invoicehd[i].hosid && checkInvEditList[i][j] == 'ข้อมูลโรงพยาบาลไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].bookNo == this.invoicehd[i].bookNo && checkInvEditList[i][j] == 'ข้อมูลเล่มที่ใบเสร็จไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].bill_no == this.invoicehd[i].receiptNo && checkInvEditList[i][j] == 'ข้อมูลเลขที่ใบเสร็จไม่ตรงกับภาพถ่ายไม่เสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].typePatient == this.invoicehd[i].victimType && checkInvEditList[i][j] == 'ข้อมูลประเภทผู้ป่วยไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].hospitalized_date == moment(this.invoicehd[i].takendate).format("YYYY-MM-DD") && checkInvEditList[i][j] == 'ข้อมูลวันที่เข้ารักษาไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].hospitalized_time == this.invoicehd[i].takentime.replace('.', ':') && checkInvEditList[i][j] == 'ข้อมูลเวลาที่เข้ารักษาไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].out_hospital_date == moment(this.invoicehd[i].dispensedate).format("YYYY-MM-DD") && checkInvEditList[i][j] == 'ข้อมูลวันที่ออกจากโรงพยาบาลไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (this.bills[i].out_hospital_time == this.invoicehd[i].dispensetime.replace('.', ':') && checkInvEditList[i][j] == 'ข้อมูลเวลาที่ออกจากโรงพยาบาลไม่ตรงกับภาพถ่ายใบเสร็จ') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบ' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (!this.bills[i].isEditImage && checkInvEditList[i][j] == 'ภาพถ่ายไม่ใช่ใบเสร็จค่ารักษา') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบรายละเอียด เนื่องจาก' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    } else if (!this.bills[i].isEditImage && checkInvEditList[i][j] == 'ภาพถ่ายใบเสร็จไม่ชัด') {
                                        this.swalValidateAndCheckEdit('ตรวจสอบรายละเอียด เนื่องจาก' + this.invNotPassTypesList[l].typeName)
                                        return false
                                    }
                                }
                            }
                        }
                    }
                    if (this.bills[i].isEditImage && (this.bills[i].editBillImage == "" || this.bills[i].editBillImage == null)) {
                        this.$swal({
                            icon: 'warning',
                            text: 'กรุณาอัพโหลดรูปภาพใบเสร็จค่ารักษาให้ครบถ้วน',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {

                            }
                        })
                        return false;
                    } else if (this.bills[i].money == "0") {
                        this.$swal({
                            icon: 'warning',
                            text: 'กรุณาระบุจำนวนเงินให้ครบถ้วน',
                            showCancelButton: false,
                            showDenyButton: false,
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {

                            }
                        })
                        return false;
                    }
                }

                var checkBankAccountEditList = this.bookbankNotPassDescList
                if (checkBankAccountEditList) {
                    for (let i = 0; i < checkBankAccountEditList.length; i++) {
                        for (let j = 0; j < this.bankAccountNotPassTypesList.length; j++) {
                            if (checkBankAccountEditList[i] == this.bankAccountNotPassTypesList[j].typeName) {
                                if (this.inputBank.accountBankName == this.beforeInputBank.accountBankName && checkBankAccountEditList[i] == 'ข้อมูลชื่อธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร') {
                                    this.swalValidateAndCheckEdit('ตรวจสอบ' + this.bankAccountNotPassTypesList[j].typeName)
                                    return false
                                } else if (this.inputBank.accountName == this.beforeInputBank.accountName && checkBankAccountEditList[i] == 'ข้อมูลชื่อบัญชีไม่ตรงกับภาพถ่ายบัญชีธนาคาร') {
                                    this.swalValidateAndCheckEdit('ตรวจสอบ' + this.bankAccountNotPassTypesList[j].typeName)
                                    return false
                                } else if (this.inputBank.accountNumber == this.beforeInputBank.accountNumber && checkBankAccountEditList[i] == 'ข้อมูลเลขที่บัญชีธนาคารไม่ตรงกับภาพถ่ายบัญชีธนาคาร') {
                                    this.swalValidateAndCheckEdit('ตรวจสอบ' + this.bankAccountNotPassTypesList[j].typeName)
                                    return false
                                } else if (!this.inputBank.isEditBankImage && checkBankAccountEditList[i] == 'ภาพถ่ายไม่ใช่บัญชีธนาคาร') {
                                    this.swalValidateAndCheckEdit('ตรวจสอบ' + this.bankAccountNotPassTypesList[j].typeName)
                                    return false
                                } else if (!this.inputBank.isEditBankImage && checkBankAccountEditList[i] == 'ภาพถ่ายบัญชีธนาคารไม่ชัด') {
                                    this.swalValidateAndCheckEdit('ตรวจสอบ' + this.bankAccountNotPassTypesList[j].typeName)
                                    return false
                                }
                            }
                        }
                    }
                }
                if (this.inputBank.isEditBankImage && (this.inputBank.bankBase64String == "" || this.inputBank.bankBase64String == null)) {
                    this.$swal({
                        icon: 'warning',
                        text: 'กรุณาอัพโหลดรูปภาพบัญชีธนาคารให้ครบถ้วน',
                        showCancelButton: false,
                        showDenyButton: false,
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {

                        }
                    })
                    return false;
                }
                for (let i = 0; i < this.bills.length; i++) {

                    this.bills[i].accNo = this.iclaimAppData.accNo
                    this.bills[i].victimNo = this.accData.victimNo
                    this.bills[i].reqNo = this.$route.params.appNo
                    this.bills[i].money = this.bills[i].money.toString()
                }

                var url = this.$store.state.envUrl + '/api/Approval/CheckInvoiceUsing'
                let isDuplicate = false;
                this.isLoading = true;
                axios.post(url, JSON.stringify(this.bills), {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': "Bearer " + this.$store.state.jwtToken.token
                    }
                }).then((response) => {
                    var billIdDuplicate = [];
                    this.duplicateInv = response.data
                    if (this.duplicateInv.length > 0) {
                        for (let i = 0; i < this.duplicateInv.length; i++) {
                            if (this.duplicateInv[i].isDuplicate == true) {
                                isDuplicate = true;
                                billIdDuplicate.push({
                                    idInvhd: this.duplicateInv[i].billId,
                                    bill_no: (i + 1)
                                });
                            }
                        }
                    }

                    if (billIdDuplicate.length > 0) {
                        this.isLoading = false;
                        var htmlMessage = '<p style="margin-bottom:10px" align="left"> <strong>ใบเสร็จที่ : ' + billIdDuplicate.map(m => m.bill_no) + ' </strong><br>&emsp;&emsp;เคยใช้ในการเบิกค่าเสียหายเบื้องต้นไปแล้ว กรุณาใช้ใบเสร็จค่ารักษาอื่น </p>'
                        htmlMessage += '<p style="color:red;margin-bottom:5px" align="left">&emsp;&emsp;*หมายเหตุ หากท่านต้องการยกเลิกใบเสร็จในการเบิกคำร้องนี้ กรุณาติ๊กเลือกใบเสร็จแล้วกดปุ่ม <label style="color:var(--main-color)">"ยกเลิกใบเสร็จ"</label> </p>'
                        for (let i = 0; i < billIdDuplicate.length; i++) {
                            htmlMessage += '<div class="form-check px-5"><input id="swal-check' + (i + 1) + '" class="form-check-input" type="checkbox" value="" ><label class="form-check-label" for="swal-check' + (i + 1) + '">ยกเลิกใบเสร็จที่ : ' + billIdDuplicate[i].bill_no + '</label></div>'
                        }

                        this.$swal({
                            icon: 'warning',
                            html: htmlMessage,
                            title: 'คำเตือน',
                            showCancelButton: false,
                            showDenyButton: true,
                            denyButtonText: "<a style='color: #5c2e91; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                            denyButtonColor: '#dad5e9',
                            confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ยกเลิกใบเสร็จ",
                            confirmButtonColor: '#5c2e91',
                            willClose: () => {

                            }
                        }).then((result) => {
                            if (result.isConfirmed) {
                                for (let i = 0; i < billIdDuplicate.length; i++) {
                                    if (document.getElementById('swal-check' + (i + 1)).checked) {
                                        let index = this.bills.findIndex(w => w.billNo === billIdDuplicate[i].idInvhd)
                                        this.bills[index].isCancel = true
                                        this.invOverlay[index].isShow = true
                                    }
                                }
                                var billCansel = this.bills.filter(w => w.isCancel).length
                                if (billCansel == this.iclaimAppData.iclaimInvCount) {
                                    this.$swal({
                                        icon: 'warning',
                                        html: '<p style="margin-bottom:10px" align="left">&emsp;&emsp;เนื่องจากคำร้องของท่านมีใบเสร็จค่ารักษาพยาบาลอยู่ ' + this.iclaimAppData.iclaimInvCount + ' ใบเสร็จ และท่านต้องการจะยกเลิกใบเสร็จทั้งหมด หมายความว่าท่านต้องการจะยกเลิกคำร้องนี้. </p>',
                                        title: 'คำเตือน',
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
                                        this.$store.state.inputApprovalData.SumMoney = null
                                        this.$store.state.inputApprovalData.ClaimNo = null
                                        this.$store.state.inputApprovalData.UserIdLine = this.$store.state.userTokenLine
                                        this.$store.state.inputApprovalData.Injury = null
                                        this.$store.state.inputApprovalData.BillsData = null
                                        this.$store.state.inputApprovalData.BankData = null
                                        this.$store.state.inputApprovalData.VictimData = null
                                        if (result.isConfirmed) {
                                            this.$router.push({ name: 'ConfirmOTP', params: { from: "CanselApproval" } })
                                        } else {
                                            for (let i = 0; i < billIdDuplicate.length; i++) {
                                                let index = this.bills.findIndex(w => w.billNo === billIdDuplicate[i].idInvhd)
                                                this.bills[index].isCancel = false
                                                this.invOverlay[index].isShow = false
                                            }
                                        }
                                    });
                                }
                            }
                        });
                        return false;
                    } else {
                        this.isLoading = false;
                        this.$swal({
                            icon: 'question',
                            text: 'ท่านยืนยันที่จะส่งเอกสารเพิ่มเติมหรือไม่?',
                            /*title: 'แจ้งเตือน',*/
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
                            if (result.isConfirmed) {
                                this.$router.push({ name: 'ConfirmOTP', params: { from: "Edit" } })
                            }
                            //} else if (result.isDenied) {

                            //}
                        });
                    }


                }).catch((error) => {
                    this.isLoading = false;
                    if (error.toString().includes("401")) {
                        this.getJwtToken()
                    }
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

                    sum = sum + parseFloat(this.bills[i].money)
                }
                this.total_amount = sum.toFixed(2)

            },
            rmLeadingZero(index) {
                let sum = 0;
                for (let i = 0; i < this.bills.length; i++) {
                    if (i != index) {
                        sum = sum + parseFloat(this.bills[i].money)
                    }
                }
                if ((sum + parseFloat(this.bills[index].money)) > 2000) {
                    this.bills[index].money = 2000 - sum
                    this.bills[index].money = this.bills[index].money.toString()
                    this.calMoney()
                } else {
                    this.bills[index].money = parseFloat(this.bills[index].money)
                    this.bills[index].money = this.bills[index].money.toString()
                    this.calMoney()
                }
            },
            addField(value, fieldType) {
                let sum = 0;
                for (let i = 0; i < this.bills.length; i++) {

                    sum = sum + parseFloat(this.bills[i].money)
                }
                if (sum >= 2000) {
                    this.$swal({
                        icon: 'warning',
                        text: 'ไม่สามารถเพิ่มใบเสร็จได้ เนื่องจากจำนวนเงินถึง 2000 บาทแล้ว',
                        /*footer: '<a href="">Why do I have this issue?</a>'*/
                        showCancelButton: false,
                        showDenyButton: false,
                        confirmButtonText: "<a style='color: white; text-decoration: none; font-family: Mitr; font-weight: bold; border-radius: 4px;'>ปิด",
                        confirmButtonColor: '#5c2e91',
                        willClose: () => {
                        }
                    })
                    return false
                }
                var index = this.bills.length + 1
                fieldType.push({
                    billNo: index, bill_no: "", bookNo: "", selectHospital: '', money: "0", hospitalized_date: "", hospitalized_time: "",
                    out_hospital_date: "", out_hospital_time: "", typePatient: "OPD", injuri: "", injuriId: "", selectHospitalId: "",
                    file: null, billFileShow: "", filename: "", editBillImage: "", isEditImage: false, displayBtnChangeBillImage: "แก้ไขรูปใบเสร็จ",
                    isCancel: false
                });
                this.calMoney()
            },
            removeField(index, fieldType) {
                //type.splice(index, 1);
                fieldType.splice(index, 1);
                this.calMoney()
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
                        this.$router.push({ name: 'Approvals', params: { id: this.accData.stringAccNo } })
                    }
                })
            },
            showBigImage(src) {
                this.modalBigImage = true
                this.srcBigImage = src
            },
            calIndex(index) {
                return parseInt(index) + 1;
            },

        },
        async created() {
            await this.getHospitalNames();
            /*await this.getDocumentCheck();*/

            this.selectChangwat = 0;
            this.selectHospital = 0;
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